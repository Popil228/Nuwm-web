using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Microsoft.EntityFrameworkCore;
using static Project1.Models.DTO.SetScheduleDto;
using static Project1.Models.DTO.RequestDto;
using Project1.Models.DTO;
using Project1.Models.Entitys;

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

        [HttpPost("schedule")]
        public async Task<ActionResult> AddSchedule([FromBody] ScheduleFormDto scheduleData)
        {
            try
            {
                var subgroup = scheduleData.SubgroupId.HasValue ? await _dbContext.Subgroups.FindAsync(scheduleData.SubgroupId.Value) : null;
                var subject = await _dbContext.Subjects.FindAsync(scheduleData.SubjectId);
                var pairNumber = await _dbContext.NumParas.FindAsync(scheduleData.NumParaId);
                var type = await _dbContext.TypeParas.FindAsync(scheduleData.TypeParaId);

                if ( subject == null || pairNumber == null || type == null || subgroup == null)
                {
                    return BadRequest("Некоректні дані для створення розкладу");
                }

                // Створення нового запису в таблиці розкладу
                var newSchedule = new Schedule
                {
                    SubgroupId = (int)scheduleData.SubgroupId,
                    SubjectId = scheduleData.SubjectId,
                    NumParaId = scheduleData.NumParaId,
                    TypeParaId = scheduleData.TypeParaId,
                    Cabinet = scheduleData.Cabinet,
                    Data = scheduleData.Data
                };

                // Додавання розкладу в базу даних
                _dbContext.Schedules.Add(newSchedule);

                // Збереження змін у БД
                await _dbContext.SaveChangesAsync();

                return Json("Розклад успішно додано");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteLastSchedule()
        {
            try
            {
                // Отримуємо останній доданий розклад
                var lastSchedule = await _dbContext.Schedules
                    .OrderByDescending(s => s.Id)  // або s.CreatedAt, якщо таке поле є
                    .FirstOrDefaultAsync();

                if (lastSchedule == null)
                {
                    return NotFound("Розклад не знайдений.");
                }

                // Видаляємо останній розклад
                _dbContext.Schedules.Remove(lastSchedule);
                await _dbContext.SaveChangesAsync();

                return Json("Останній розклад успішно видалено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}
