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
        [HttpPost("data")]
        public async Task<ActionResult<TeacherPageDto>> GetTeacherPageData()
        {
            try
            {
                // Обробка запиту, якщо потрібно врахувати якісь додаткові параметри з тіла запиту
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
        [HttpPost("subgroups_teachers")]
        public async Task<ActionResult<List<SelectItemDto>>> GetSubgroupsForGroup([FromBody] GroupRequest groupRequest)
        {
            try
            {
                var subgroups = await _dbContext.Subgroups
                    .Where(s => s.GroupId == groupRequest.GroupId)
                    .Select(s => new SelectItemDto
                    {
                        Value = s.Id.ToString(),
                        Label = s.Number.ToString()
                    })
                    .ToListAsync();

                var teachers = await _dbContext.TeacherGroups
                    .Where(tg => tg.GroupId == groupRequest.GroupId)  // Фільтруємо по GroupId
                    .Include(tg => tg.Teacher)  // Завантажуємо викладачів, що належать до цієї групи
                    .ThenInclude(teacher => teacher.Person)  // Завантажуємо пов'язану особу для кожного викладача
                    .Select(tg => new SelectItemDto
                    {
                        Value = tg.Teacher.Id.ToString(),  // Отримуємо Id викладача
                        Label = $"{tg.Teacher.Person.ThirdName} {tg.Teacher.Person.Name} {tg.Teacher.Person.SurName}"  // Формуємо строку з ПІБ викладача
                    })
                    .ToListAsync();


                var result = new TeacherPageDto { Subgroups =  subgroups, Teachers = teachers };

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // Отримати дисципліни для вибраного викладача
        [HttpPost("subjects")]
        public async Task<ActionResult<List<SelectItemDto>>> GetSubjectsForTeacher([FromBody] TeacherRequest teacherRequest)
        {
            try
            {
                var subjects = await _dbContext.Subjects
                    .Where(s => s.TeacherId == teacherRequest.TeacherId)
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
        public class GroupRequest
        {
            public int GroupId { get; set; }
        }

        public class TeacherRequest
        {
            public int TeacherId { get; set; }
        }
    }
}
