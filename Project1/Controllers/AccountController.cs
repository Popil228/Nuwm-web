using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using System.IdentityModel.Tokens.Jwt;
using Project1.Models.DTO;

namespace Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
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
                                .Include(p => p.Student) // Підключаємо студента
                                .ThenInclude(s => s.Group) // Підключаємо групу студента
                                .ThenInclude(s => s.Subgroups) // Підключаємо підгрупи групи
                                .Include(p => p.Student)
                                .ThenInclude(s => s.Group)
                                .ThenInclude(g => g.Institute)
                                .SingleOrDefaultAsync(p => p.Email == emailClaim);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var subgroup = user.Student.Group.Subgroups.FirstOrDefault();
            var course = user.Student.Group.Course;

            // Формуємо DTO для повернення
            var person = new PersonDto
            {
                Name = user.Name,
                SurName = user.SurName,
                ThirdName = user.ThirdName,
                Group = user.Student.Group.Name,
                Subgroup = user.Student.Subgroup.Number.ToString(), // Перевіряємо, чи є підгрупи
                Course = course.ToString(),
                Institute = user.Student.Group.Institute?.Name ?? "No Institute" // Перевіряємо, чи є інститут
            };



            return Json(person);  // Повертаємо дані користувача разом з його деталями
        }

        [HttpGet("student-total-scores")]
        public async Task<IActionResult> GetStudentTotalScores()
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

            // Знаходимо ID студента за його електронною поштою
            var student = await _context.Persons
                .Include(p => p.Student)
                .SingleOrDefaultAsync(p => p.Email == emailClaim);

            if (student == null || student.Student == null)
            {
                return NotFound("Student not found.");
            }

            // ID студента
            var studentId = student.Student.Id;

            // Отримуємо сумарні бали для кожного предмету конкретного студента
            var totalScores = await _context.Marks
                .Include(m => m.Subject) // Підключаємо предмети
                .Where(m => m.StudentId == studentId) // Фільтруємо за ID студента
                .GroupBy(m => m.Subject.Title) // Групуємо за назвою предмету
                .Select(g => new StudentMarksDto
                {
                    SubjectName = g.Any()
                        ? g.Key // Назва предмету, якщо є оцінки
                        : "Оцінок ще не виставлено", // Якщо немає оцінок, відображаємо цей текст
                    Value = g.Any()
                        ? g.Sum(m => m.Value) // Сумарний бал, якщо є оцінки
                        : 0 // Якщо немає оцінок, ставимо 0
                })
                .ToListAsync();

            return Json(totalScores);
        }

        [HttpGet("group-student-total-scores")]
        public async Task<IActionResult> GetGroupTotalScores()
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

            // Знаходимо студента по email
            var student = await _context.Persons
                .Include(p => p.Student)
                .ThenInclude(s => s.Group)
                .SingleOrDefaultAsync(p => p.Email == emailClaim);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var groupId = student.Student.GroupId;  // Отримуємо GroupId студента

            // Отримуємо суми балів для студентів в цій групі
            var groupScores = await _context.Marks
                .Include(m => m.Student)
                .Where(m => m.Student.GroupId == groupId)  // Фільтруємо за GroupId
                .GroupBy(m => m.StudentId)
                .Select(g => new GroupMarksDto
                {
                    StudentName = g.Any()
                        ? $"{g.FirstOrDefault().Student.Person.SurName} {g.FirstOrDefault().Student.Person.Name} {g.FirstOrDefault().Student.Person.ThirdName}"
                        : "Оцінок ще не виставлено", // Якщо немає оцінок, відображаємо цей текст
                    TotalScore = g.Any()
                        ? g.Sum(m => (int)Math.Round(m.Value))
                        : 0  // Якщо немає оцінок, ставимо 0
                })
                .ToListAsync();

            if (groupScores == null || !groupScores.Any())
            {
                return NotFound("No student data found for this group.");
            }

            return Json(groupScores); // Повертаємо результат у вигляді DTO
        }

    }
}
