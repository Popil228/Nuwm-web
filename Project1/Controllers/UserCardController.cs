using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetUserData()
        {
            // Отримуємо токен з заголовка
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (emailClaim == null)
            {
                return Unauthorized("Email not found in token.");
            }

            // Пошук користувача в базі даних разом з його деталями
            var user = await _context.Persons
                                      .Include(u => u.Student.Group.Institute)
                                      .Include(u => u.Student.Group.Subgroups)
                                      .FirstOrDefaultAsync(u => u.Email == emailClaim);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);  // Повертаємо дані користувача разом з його деталями
        }

    }
}
