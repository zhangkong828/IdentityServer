using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using System.Security.Claims;
using IdentityServer.Service;
using IdentityModel;
using IdentityServer.Entity;

namespace IdentityServer.Web.Ids4
{
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class Ids4Controller : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IIdentityServerInteractionService _idsInteraction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        public Ids4Controller(IUserAccountService userAccountService, IIdentityServerInteractionService idsInteraction, IAuthenticationSchemeProvider schemeProvider)
        {
            _userAccountService = userAccountService;

            _idsInteraction = idsInteraction;
            _schemeProvider = schemeProvider;
        }

        [HttpGet]
        [Route("/login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var login = await _idsInteraction.GetAuthorizationContextAsync(returnUrl);
            //if (string.IsNullOrWhiteSpace(login.IdP) == false)
            //{
            //    return ExternalLogin(login.IdP, resumeUrl);
            //}

            var model = new LoginViewModel(null)
            {
                ReturnUrl = returnUrl,
                //default username and password
                UserName = "test",
                Password = "123456",
                ExternalLoginList = await GetExternalLoginViewModels(returnUrl)
            };

            return View(model);
        }

        private async Task<List<ExternalLoginViewModel>> GetExternalLoginViewModels(string returnUrl)
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var externals = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalLoginViewModel
                {
                    DisplayName = x.DisplayName,
                    LoginUrl = Url.Action(nameof(ExternalLogin), new
                    {
                        scheme = x.Name,
                        returnUrl
                    })
                }).ToList();
            return externals;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginFormModel form)
        {
            if (ModelState.IsValid == false)
            {
                return View(new LoginViewModel(form)
                {
                    ExternalLoginList = await GetExternalLoginViewModels(form.ReturnUrl)
                });
            }

            if (_userAccountService.ValidateCredentials(form.UserName, form.Password, HttpContext.GetIpAddress(), out UserAccount user))
            {
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };
                var isuser = new IdentityServerUser(user.UserId)
                {
                    DisplayName = user.NickName
                };
                await HttpContext.SignInAsync(isuser, properties);

                if (_idsInteraction.IsValidReturnUrl(form.ReturnUrl) || Url.IsLocalUrl(form.ReturnUrl))
                {
                    return Redirect(form.ReturnUrl);
                }

                return Redirect("~/");
            }
            else
            {
                ViewBag.Error = "invalid username or password.";
                return View(new LoginViewModel(form)
                {
                    ExternalLoginList = await GetExternalLoginViewModels(form.ReturnUrl)
                });
            }
        }

        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _idsInteraction.GetLogoutContextAsync(logoutId);

            await HttpContext.SignOutAsync();

            ViewBag.SignOutIframeUrl = logout.SignOutIFrameUrl;
            ViewBag.RedirectUri = new Uri(logout.PostLogoutRedirectUri).GetLeftPart(UriPartial.Authority);

            return View("LoggedOut");
        }

        [HttpGet]
        [Route("external-login/{scheme}", Name = "external-login")]
        public IActionResult ExternalLogin(string scheme, string returnUrl)
        {
            returnUrl = Url.Action(nameof(ExternalLoginCallback), new { returnUrl });
            var props = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                Items = { { "scheme", scheme } }
            };
            return new ChallengeResult(scheme, props);
        }

        [HttpGet]
        [Route("external-login/callback")]
        public async Task<IActionResult> ExternalLoginCallback(string resumeUrl)
        {
            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);
            var user = _userAccountService.QueryUserByUserId(userId);
            if (user != null)
            {
                await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                var isuser = new IdentityServerUser(user.UserId)
                {
                    DisplayName = user.NickName,
                    IdentityProvider = scheme,
                    AdditionalClaims = claims
                };
                await HttpContext.SignInAsync(isuser);
                return Redirect(resumeUrl);
            }

            ViewBag.NickName = GetUserNickName(claims);
            ViewBag.AvatarUrl = GetUserAvatar(claims);
            return View("ExternalLoginNewUser");
        }

        [HttpPost]
        [Route("external-login/callback")]
        public async Task<ActionResult> ExternalLoginCreateNewUser(string returnUrl, [FromForm] NewUserViewModel viewModel)
        {
            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);

            var user = _userAccountService.QueryUserByUserId(userId); //_userStore.AutoProvisionUser(scheme, userId, claims);
            user.NickName = viewModel.UserName;

            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var isuser = new IdentityServerUser(user.UserId)
            {
                DisplayName = user.NickName,
                IdentityProvider = scheme,
                AdditionalClaims = claims
            };
            await HttpContext.SignInAsync(isuser);

            return Redirect(returnUrl);
        }

        [HttpGet]
        [Route("js")]
        public ActionResult IsLogin()
        {

            var isLogin = base.User?.Identity?.IsAuthenticated ?? false;
            var javaScript = "var account={"
                           + "is_login:" + isLogin.ToString().ToLower()
                           + "};";
            return Content(javaScript, "application/javascript");
        }

        private static string GetScheme(AuthenticateResult externalLogin)
        {
            return externalLogin.Properties.Items["scheme"];
        }

        private static List<Claim> GetClaims(AuthenticateResult externalLogin)
        {
            var tempUser = externalLogin?.Principal;
            if (tempUser == null)
            {
                throw new Exception("External authentication error");
            }

            return tempUser.Claims.ToList();
        }

        private static string GetUserId(ICollection<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("Unknown userid");
            }
            return userIdClaim.Value;
        }

        private static string GetUserNickName(ICollection<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == JwtClaimTypes.NickName)?.Value;
        }

        private static string GetUserAvatar(ICollection<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == "avatar")?.Value;
        }
    }
}
