using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Project1.Data;
using Project1.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GetScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("date")]
        public async Task<IActionResult> GetSchedulesByDate([FromBody] string Data)
        {
            if (string.IsNullOrEmpty(Data))
            {
                return BadRequest("Date is required.");
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var student = await _context.Persons
                .Where(p => p.Email == emailClaim)  // Фільтруємо по Email
                .Include(p => p.Student)            // Підключаємо студента
                    .ThenInclude(s => s.Group)      // Підключаємо групу студента
                        .ThenInclude(g => g.Subgroups)  // Підключаємо підгрупи
                            .ThenInclude(sg => sg.Schedules) // Завантажуємо розклад підгрупи
                            .ThenInclude(sch => sch.NumPara)
                                .ThenInclude(sg => sg.Schedules) // Завантажуємо розклад підгрупи
                                .ThenInclude(sch => sch.TypePara)
                                    .ThenInclude(sg => sg.Schedules)
                                    .ThenInclude(sch => sch.Subject)  // Завантажуємо предмети
                                        .ThenInclude(sub => sub.Teacher) // Завантажуємо викладача
                                            .ThenInclude(tchr => tchr.Person)  // Завантажуємо ПІБ викладача
                .Where(p => p.Student.Group.Subgroups.Any(sg => sg.Schedules.Any(schedule => schedule.Data == Data))) // Фільтрація по Data для розкладу
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Витягуємо розклад для студента
            var scheduleDtos = student.Student
                .Group // Отримуємо групу студента
                .Subgroups // Отримуємо всі підгрупи
                .SelectMany(sg => sg.Schedules) // Отримуємо всі записи розкладу
                .Where(schedule => schedule.Data == Data) // Фільтруємо по датах
                .Select(schedule => new GetScheduleDto
                {
                    NumParaID = schedule.NumPara.Number,
                    NumPara = schedule.NumPara.Time, // Час пари
                    Groups = schedule.Subgroup.Group.Name,
                    TypePara = schedule.TypePara.Title, // Тип пари
                    Cabinet = schedule.Cabinet.ToString(), // Кабінет
                    Subject = schedule.Subject.Title, // Назва предмету
                    TeacherName = $"{schedule.Subject.Teacher.Person.SurName} {schedule.Subject.Teacher.Person.Name} {schedule.Subject.Teacher.Person.ThirdName}".Trim() // ПІБ викладача
                })
                .OrderBy(dto => dto.NumParaID) // Сортуємо за NumParaID
                .ToList();

            if(scheduleDtos == null)
            {
                return Json("На цей день розкладу немає");
            }
            else{
                return Json(scheduleDtos);
            }
        }

    }
}
