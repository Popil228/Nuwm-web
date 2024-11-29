using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Microsoft.EntityFrameworkCore;
using static Project1.Models.DTO.SetScheduleDto;

namespace Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetScheduleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public SetScheduleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Отримання даних для форми
        [HttpGet("data")]
        public async Task<ActionResult<TeacherPageDto>> GetTeacherPageData()
        {
            try
            {
                var groups = await _dbContext.Groups
                    .Select(g => new SelectItemDto
                    {
                        Value = g.Id.ToString(),
                        Label = g.Name
                    })
                    .ToListAsync();

                var pairNumbers = await _dbContext.NumParas
                    .Select(p => new SelectItemDto
                    {
                        Value = p.Id.ToString(),
                        Label = $"Пара {p.Number} ({p.Time})"
                    })
                    .ToListAsync();

                var types = await _dbContext.TypeParas
                    .Select(t => new SelectItemDto
                    {
                        Value = t.Id.ToString(),
                        Label = t.Title
                    })
                    .ToListAsync();

                var result = new TeacherPageDto
                {
                    Groups = groups,
                    PairNumbers = pairNumbers,
                    Types = types
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // Отримати підгрупи для вибраної групи
        [HttpGet("subgroups/{groupId}")]
        public async Task<ActionResult<List<SelectItemDto>>> GetSubgroupsForGroup(int groupId)
        {
            try
            {
                var subgroups = await _dbContext.Subgroups
                    .Where(s => s.GroupId == groupId)
                    .Select(s => new SelectItemDto
                    {
                        Value = s.Id.ToString(),
                        Label = s.Number.ToString()
                    })
                    .ToListAsync();

                return Json(subgroups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // Отримати викладачів для вибраної групи
        [HttpGet("teachers/{groupId}")]
        public async Task<ActionResult<List<SelectItemDto>>> GetTeachersForGroup(int groupId)
        {
            try
            {
                var teachers = await _dbContext.Teachers
                    .Where(t => t.TeacherGroups.Any(tg => tg.GroupId == groupId))
                    .Join(
                        _dbContext.Persons,
                        teacher => teacher.PersonId,
                        person => person.Id,
                        (teacher, person) => new SelectItemDto
                        {
                            Value = teacher.Id.ToString(),
                            Label = $"{person.ThirdName} {person.Name} {person.SurName}"
                        })
                    .ToListAsync();

                return Json(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // Отримати дисципліни для вибраного викладача
        [HttpGet("subjects/{teacherId}")]
        public async Task<ActionResult<List<SelectItemDto>>> GetSubjectsForTeacher(int teacherId)
        {
            try
            {
                var subjects = await _dbContext.Subjects
                    .Where(s => s.TeacherId == teacherId)  // Використовуємо TeacherId для фільтрації
                    .Select(s => new SelectItemDto
                    {
                        Value = s.Id.ToString(),
                        Label = s.Title
                    })
                    .ToListAsync();

                return Json(subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}
