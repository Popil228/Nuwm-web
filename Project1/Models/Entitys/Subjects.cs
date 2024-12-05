namespace Project1.Models.Entitys
{
    public class Subject
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        // Foreign Key для зв'язку з Teachers
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        // Навігаційна властивість для Marks і Schedule
        public ICollection<Mark>? Marks { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }

}
