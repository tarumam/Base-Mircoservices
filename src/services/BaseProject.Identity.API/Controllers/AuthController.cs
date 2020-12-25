using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BaseProject.Core.Messages.Integration;
using BaseProject.Identity.API.Extensions;
using BaseProject.Identity.API.Models;
using BaseProject.MessageBus;
using BaseProject.WebAPI.Core.Controllers;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BaseProject.Identity.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        private readonly IMessageBus _bus;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings, IMessageBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> Register(RegisterUser registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                var clientRegistered = await RegisterClient(registerUser);
                if (!clientRegistered.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clientRegistered.ValidationResult);
                }
                return CustomResponse(await GenerateJWT(registerUser.Email));
            }

            foreach (var error in result.Errors)
            {
                AddProcessingError(error.Description);
            }
            return CustomResponse();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> Login(LoginUser loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJWT(loginUser.Email));
            }

            if (result.IsLockedOut)
            {
                AddProcessingError("Você excedeu o número de tentativas de login");
                return CustomResponse();
            }
            AddProcessingError("Informações incorretas, verifique seu login e senha");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterClient(RegisterUser user)
        {
            var u = await _userManager.FindByEmailAsync(user.Email);

            var registeredUser = new UserRegisteredIntegrationEvent(Guid.Parse(u.Id), user.Name, user.Email, user.Cpf);

            try
            {
                return await _bus.RequestAsync<UserRegisteredIntegrationEvent, ResponseMessage>(registeredUser);
            }
            catch
            {
                await _userManager.DeleteAsync(u);
                throw;
            }
        }

        private async Task<LoginResponse> GenerateJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GetUserClaims(claims, user);
            string encodedToken = EncodeToken(identityClaims);

            return GetToken(user, claims, encodedToken);
        }

        private LoginResponse GetToken(IdentityUser user, IList<Claim> claims, string encodedToken)
        {
            return new LoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationTime).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userrole in userRoles)
            {
                claims.Add(new Claim("role", userrole));
            }
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
