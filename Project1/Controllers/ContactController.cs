using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly EmailService _emailService;

    public ContactController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost ("contact")]
    public async Task<IActionResult> SendMessage([FromBody] ContactFormModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");

        var subject = $"Новий контакт: {model.FirstName} {model.LastName}";
        var body = $@"
            <p><strong>Ім'я:</strong> {model.FirstName}</p>
            <p><strong>Прізвище:</strong> {model.LastName}</p>
            <p><strong>Email:</strong> {model.Email}</p>
            <p><strong>Телефон:</strong> {model.Phone}</p>
            <p><strong>Повідомлення:</strong></p>
            <p>{model.Message}</p>";

        await _emailService.SendEmailAsync("galenukm@gmail.com", subject, body);

        return Ok("Повідомлення успішно відправлено!");
    }

    public class ContactFormModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Message { get; set; }
    }

}
