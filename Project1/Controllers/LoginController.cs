using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Project1.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;


namespace Project1.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public AuthController(IMemoryCache cache, ApplicationDbContext context)
        {
            _cache = cache;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", new { returnUrl })
            };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
        {


            _cache.TryGetValue("UserPrincipal", out ClaimsPrincipal userPrincipal);

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var identity = userPrincipal.Identities.FirstOrDefault();

            if (identity == null)
            {
                return Unauthorized();
            }

            var claims = identity.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var token = GenerateJwt(userPrincipal);
            return Redirect($"https://localhost:44476/login-success?token={token}");
        }

        private string GenerateJwt(ClaimsPrincipal principal)
        {
            // Приклад генерації JWT (залежно від ваших вимог)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkdhbGVuIiwiaWF0IjoxNTE2MjM5MDIyfQ.BmYG5rn-Z47gwwGZKuPNAVgYHCMNHtDN3ZLGjekiqio");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(principal.Claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
