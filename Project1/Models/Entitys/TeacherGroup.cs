namespace Project1.Models.Entitys
{
    public class TeacherGroup
    {

        public int Id { get; set; }
        public string? GroupId { get; set; }
        public Group? Group { get; set; }
        public string? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
