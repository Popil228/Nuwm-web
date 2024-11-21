namespace Project1.Models.Entitys
{
    public class NumPara
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Time { get; set; }

        // Навігаційна властивість для зв'язку з Schedule
        public ICollection<Schedule> Schedules { get; set; }
    }

}
