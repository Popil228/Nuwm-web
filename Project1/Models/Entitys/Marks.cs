namespace Project1.Models.Entitys
{
    public class Mark
    {
        public int Id { get; set; }

        // Foreign Key для зв'язку з Students
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        // Foreign Key для зв'язку з Subjects
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public float Value { get; set; }
    }

}
