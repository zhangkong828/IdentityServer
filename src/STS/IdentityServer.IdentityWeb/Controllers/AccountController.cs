using IdentityServer.IdentityWeb.Models;
using IdentityServer.Service.Interfaces;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.EntityFramework.Entities.Identity;
using System.Security.Claims;
using IdentityModel;

namespace IdentityServer.IdentityWeb.Controllers
{
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _idsInteraction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        private readonly IIdentityService _identityService;
        public AccountController(IIdentityServerInteractionService idsInteraction, IAuthenticationSchemeProvider schemeProvider, IIdentityService identityService)
        {
            _idsInteraction = idsInteraction;
            _schemeProvider = schemeProvider;

            _identityService = identityService;
        }

        [HttpGet]
        [Route("/login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/";
            var model = new LoginViewModel(null)
            {
                ReturnUrl = returnUrl,
                //default username and password
                //UserName = "test",
                //Password = "123456",
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
                        provider = x.Name,
                        returnUrl
                    })
                }).ToList();
            return externals;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            if (_identityService.ValidateUsername(form.UserName, form.Password, HttpContext.GetIpAddress(), out UserIdentity user))
            {
                var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
                if (form.Remember)
                    expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = expires
                };
                var isuser = new IdentityServerUser(user.UserId)
                {
                    DisplayName = user.NickName,
                    AdditionalClaims = new Claim[]
                                        {
                                        new Claim(JwtClaimTypes.Id, user.UserId),
                                        new Claim(JwtClaimTypes.NickName,user.NickName),
                                        new Claim(JwtClaimTypes.GivenName, "222"),
                                        new Claim(JwtClaimTypes.FamilyName, "333"),
                                        new Claim(JwtClaimTypes.Email, "444")
                                        }
                };
                await HttpContext.SignInAsync(isuser, properties);

                if (_idsInteraction.IsValidReturnUrl(form.ReturnUrl) || Url.IsLocalUrl(form.ReturnUrl))
                {
                    return Json(new { code = 0, msg = "登录成功", returnUrl = form.ReturnUrl });
                }

                return Json(new { code = 0, msg = "登录成功", returnUrl = "/" });
            }
            else
            {
                return Json(new { code = -1, msg = "账号或密码无效" });
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
        [Route("/external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { returnUrl });
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Items = { { "loginProvider", provider } }
            };

            return Challenge(properties, provider);
        }

        [HttpGet]
        [Route("/external-login/callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, remoteError);

                return View(nameof(Login));
            }

            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var principal = externalLogin?.Principal;
            if (principal == null)
            {
                throw new Exception("External authentication error");
            }
            var externalId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var scheme = externalLogin.Properties.Items["loginProvider"];

            var user = _identityService.QueryUserByExternal(scheme, externalId);
            if (user != null)
            {
                await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                var isuser = new IdentityServerUser(user.UserId)
                {
                    DisplayName = user.NickName,
                    IdentityProvider = scheme,
                    AdditionalClaims = principal.Claims.ToList()
                };
                await HttpContext.SignInAsync(isuser);
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.NickName = principal.FindFirstValue(ClaimTypes.Name);
            ViewBag.Email = principal.FindFirstValue(ClaimTypes.Email);
            ViewBag.LoginProvider = scheme;

            return View(nameof(ExternalLoginConfirmation));
        }

        [HttpPost]
        [Route("external-login/confirmation")]
        public async Task<ActionResult> ExternalLoginConfirmation(string returnUrl, ExternalLoginConfirmationViewModel viewModel)
        {
           
            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = externalLogin?.Principal.Claims.ToList();
            //var externalId = GetUserExternalId(claims);
            var scheme = externalLogin.Properties.Items["loginProvider"];
            var nickName = viewModel.NickName;
            var avatar = viewModel.Avatar;
            var user = _identityService.AutoRegisterByExternal(scheme, "q", HttpContext.GetIpAddress(), nickName, avatar);

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

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(RegisterFormModel form)
        {
            if (!ModelState.IsValid) return Json(new { code = -1, msg = "参数错误" });

            var result = _identityService.EmailRegister(form.NickName, form.UserName, form.Password, HttpContext.GetIpAddress());

            if (result)
            {
                //自动登录 todo

                return Json(new { code = 0, msg = "注册成功" });
            }

            return Json(new { code = -1, msg = "注册失败" });
        }
    }
}
