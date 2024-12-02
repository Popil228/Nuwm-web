using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project1.Models.Entitys;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Person> _userManager;

        // Конструктор для впровадження залежностей
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<Person> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/roles
        // Отримуємо всі ролі з бази даних
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoles()
        {
            var roles = new List<string>();

            // Перевіряємо наявність усіх ролей в базі даних
            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }

            return Ok(roles); // Повертаємо ролі в форматі списку
        }

        // GET: api/roles/{email}
        // Отримуємо ролі для конкретного користувача по його email
        [HttpGet("role")]
        public async Task<ActionResult<IEnumerable<string>>> GetRolesForUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var user = await _userManager.FindByEmailAsync(email); // Знаходимо користувача по email
            if (user == null)
            {
                return NotFound($"User with email {email} not found");
            }

            var roles = await _userManager.GetRolesAsync(user); // Отримуємо ролі для цього користувача

            return Ok(roles); // Повертаємо список ролей
        }
    }
}