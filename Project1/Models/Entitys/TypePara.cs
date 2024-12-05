namespace Project1.Models.Entitys
{
    public class TypePara
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        // Навігаційна властивість для зв'язку з Schedule
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
