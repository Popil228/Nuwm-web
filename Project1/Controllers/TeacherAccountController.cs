using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models.Entitys;
using System.IdentityModel.Tokens.Jwt;
using Project1.Models.DTO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1.Ocsp;
using static Project1.Models.DTO.RequestDto;

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
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Authorization token is missing.");
            }

            try
            {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (emailClaim == null)
            {
                return Unauthorized("Email not found in token.");
            }

            var groups = await _context.Persons
                .Where(p => p.Email == emailClaim)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.TeacherGroups)
                        .ThenInclude(tg => tg.Group) // Завантажуємо групи, пов’язані з викладачем
                .SelectMany(p => p.Teacher.TeacherGroups
                .Select(tg => new { tg.Group.Id,tg.Group.Name}))
                .ToListAsync();
                return Ok(groups);
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (emailClaim == null)
            {
                return Unauthorized("Email not found in token.");
            }

            var subjects = await _context.Persons
                .Where(p => p.Email == emailClaim)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.Subjects) // Беремо всі предмети від усіх викладачів
                     .SelectMany(p => p.Teacher.Subjects) // Витягуємо предмети викладача
                     .Select(s => new { s.Id, s.Title })
                     .Distinct() // Усуваємо дублікати, якщо є
                     .ToListAsync();
            return Json(subjects);
        }

        [HttpGet("allStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var allStudents = await _context.Students
                .Include(s => s.Person)
                .Select(s => new { s.Id, Name = $"{s.Person.SurName} {s.Person.Name} {s.Person.ThirdName}".Trim() })
                .ToListAsync();

                    return Json(allStudents);
        }

        [HttpPost("students")]
        public async Task<IActionResult> GetStudents([FromBody] GroupRequest groupRequest)
        {
            var students = await _context.Students
                .Where(s => s.GroupId == groupRequest.GroupId)
                .Include(s => s.Person)
                .Select(s => new { s.Id,  Name = $"{s.Person.SurName} {s.Person.Name} {s.Person.ThirdName}".Trim() })
                .ToListAsync();

                    return Json(students);
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

