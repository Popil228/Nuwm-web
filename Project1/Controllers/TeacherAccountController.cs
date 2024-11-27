using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models.DTO;
using Project1.Models.Entitys;
using System.IdentityModel.Tokens.Jwt;

namespace Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherAccountController(ApplicationDbContext context)
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
                                .SingleOrDefaultAsync(p => p.Email == emailClaim);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Формуємо DTO для повернення
            var person = new PersonDto
            {
                Name = user.Name,
                SurName = user.SurName,
                ThirdName = user.ThirdName
            };

            return Json(person);  // Повертаємо дані користувача разом з його деталями
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _context.Groups
                .Select(g => new { g.Id, g.Name })
                .ToListAsync();
            return Json(groups);
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.Person)
                .Select(s => new { s.Id, Name = $"{s.Person.SurName} {s.Person.Name} {s.Person.ThirdName}" })
                .ToListAsync();
            return Json(students);
        }

        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _context.Subjects
                .Select(s => new { s.Id, s.Title })
                .ToListAsync();
            return Json(subjects);
        }

        [HttpPost("add-grade")]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto gradeDto)
        {
            var grade = new Mark
            {
                StudentId = gradeDto.StudentId,
                SubjectId = gradeDto.SubjectId,
                Value = gradeDto.Value
            };

            _context.Marks.Add(grade);
            await _context.SaveChangesAsync();

            return Ok("Grade added successfully");
        }

        [HttpDelete("delete-grade")]
        public async Task<IActionResult> DeleteGrade([FromBody] GradeDeleteDto request)
        {
            var grade = await _context.Marks
                .Where(m => m.StudentId == request.StudentId && m.SubjectId == request.SubjectId)
                .OrderByDescending(m => m.Id) // Сортування за спаданням ID
                .FirstOrDefaultAsync(); // Беремо перший запис зі списку

            if (grade == null)
                return NotFound("Grade not found");

            _context.Marks.Remove(grade);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

