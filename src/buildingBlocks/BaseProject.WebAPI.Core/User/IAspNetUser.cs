using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BaseProject.WebAPI.Core.User
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserMail();
        string GetUserToken();
        bool IsAuthenticated();
        bool HasRole(string role);
        IEnumerable<Claim> GetClaims();
        HttpContext GetHttpContext();
    }
}
