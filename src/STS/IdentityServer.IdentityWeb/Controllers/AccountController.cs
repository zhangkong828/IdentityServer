﻿using IdentityServer.IdentityWeb.Models;
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
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Service.Dtos.Identity;

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

            var model = new LoginViewModel(null)
            {
                ReturnUrl = returnUrl,
                //default username and password
                //UserName = "test",
                //Password = "123456",
                ExternalLoginList = externals
            };

            return View(model);
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            if (_identityService.ValidateUsername(form.UserName, form.Password, HttpContext.GetIpAddress(), out UserIdentityDto user))
            {
                var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
                if (form.Remember)
                    expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));

                var claims = new Claim[]
                                        {
                                        new Claim(JwtClaimTypes.NickName,user.NickName),
                                        new Claim(JwtClaimTypes.Email, user.Email),
                                        new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                                        new Claim(JwtClaimTypes.Picture, user.Avatar)
                                        };

                await UserLogin(user, expires, IdentityServerConstants.LocalIdentityProvider, claims);

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

                var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
                await UserLogin(user, expires, scheme, principal.Claims.ToList());

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
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var user = _identityService.QueryUserByUsername(viewModel.Email);
            if (user != null)
            {
                return Json(new { code = -1, msg = "邮箱已存在" });
            }

            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var principal = externalLogin?.Principal;
            if (principal == null)
            {
                throw new Exception("External authentication error");
            }

            var externalId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var scheme = externalLogin.Properties.Items["loginProvider"];

            user = _identityService.AutoRegisterByExternal(scheme, externalId, HttpContext.GetIpAddress(), viewModel.NickName, viewModel.Email);

            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
            await UserLogin(user, expires, scheme, principal.Claims.ToList());

            return Json(new { code = 0, msg = "成功" });
        }

        [NonAction]
        private async Task UserLogin(UserIdentityDto user, DateTimeOffset expires, string scheme, ICollection<Claim> claims)
        {
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = expires
            };
            var isuser = new IdentityServerUser(user.UserId)
            {
                DisplayName = user.NickName,
                IdentityProvider = scheme,
                AdditionalClaims = claims
            };
            await HttpContext.SignInAsync(isuser, properties);
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(RegisterFormModel form)
        {
            if (!ModelState.IsValid) return Json(new { code = -1, msg = "参数错误" });

            var user = _identityService.QueryUserByUsername(form.UserName);
            if (user != null)
            {
                return Json(new { code = -1, msg = "用户已存在" });
            }

            var result = _identityService.EmailRegister(form.NickName, form.UserName, form.Password, HttpContext.GetIpAddress(), out user);

            if (result)
            {
                var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(2));
                var claims = new Claim[]
                                        {
                                        new Claim(JwtClaimTypes.NickName,user.NickName),
                                        new Claim(JwtClaimTypes.Email, user.Email),
                                        new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                                        new Claim(JwtClaimTypes.Picture, user.Avatar)
                                        };
                await UserLogin(user, expires, IdentityServerConstants.LocalIdentityProvider, claims);

                return Json(new { code = 0, msg = "注册成功" });
            }

            return Json(new { code = -1, msg = "注册失败" });
        }
    }
}
