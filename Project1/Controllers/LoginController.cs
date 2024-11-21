using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Project1.Data;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IMemoryCache _cache;

        public LoginController(IMemoryCache cache, ApplicationDbContext context)
        {
            _cache = cache;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {

            
            return Challenge(new AuthenticationProperties { RedirectUri = "/login/signin-google2" }, GoogleDefaults.AuthenticationScheme);

            

        }

        [HttpGet("signin-google2")]
        public async Task<IActionResult> GoogleCallback()
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

            return Json(claims);
        }


    }
}
