using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaseProject.Core.Communication;
using BaseProject.WebApp.MVC.Extensions;
using BaseProject.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace BaseProject.WebApp.MVC.Services
{
    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin usuarioLogin)
        {
            var loginContent = GetContent(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/auth/sign-in", loginContent);

            if (!HandleErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjetoResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> SignUp(UserSignUp usuarioRegistro)
        {
            var registroContent = GetContent(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/auth/sign-up", registroContent);

            if (!HandleErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjetoResponse<UserResponseLogin>(response);
        }
    }
}
