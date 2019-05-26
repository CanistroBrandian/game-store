using GameStore.Web.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace GameStore.Web.Services.Concrete
{
    public class CookieAuthProvider : IAuthProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieAuthProvider(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool Authenticate(string username, string password)
        {
            var context = _httpContextAccessor.HttpContext;
            var confUsername = _configuration.GetSection("Authentication").GetValue<string>("Username");
            var confPassword = _configuration.GetSection("Authentication").GetValue<string>("Password");
            if (confUsername.ToUpper() != username.ToUpper() || confPassword.ToUpper() != password.ToUpper())
            {
                return false;
            }
            var claims = new[] { new Claim(ClaimTypes.Name, username) };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
            return true;
        }
    }
}