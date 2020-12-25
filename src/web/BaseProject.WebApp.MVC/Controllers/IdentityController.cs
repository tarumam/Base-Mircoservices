using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Models;
using BaseProject.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebApp.MVC.Controllers
{
    public class IdentityController : MainController
    {
        private readonly IAuthService _authSvc;

        public IdentityController(IAuthService authSvc)
        {
            _authSvc = authSvc;
        }

        [HttpGet, Route("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost, Route("signup")]
        public async Task<IActionResult> SignUp(UserSignUp userSignUp)
        {
            if (!ModelState.IsValid) return View(userSignUp);

            var resposta = await _authSvc.SignUp(userSignUp);

            if (ResponseHasErrors(resposta.ResponseResult)) return View(userSignUp);

            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet, Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(UserLogin usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _authSvc.Login(usuarioLogin);

            if (ResponseHasErrors(resposta.ResponseResult)) return View(usuarioLogin);

            await RealizarLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalogo");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Catalogo");
        }

        private async Task RealizarLogin(UserResponseLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
