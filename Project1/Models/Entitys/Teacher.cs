namespace Project1.Models.Entitys
{
    public class Teacher
    {

        public int Id { get; set; }
        public string? PersonId { get; set; }
        public Person? Person { get; set; }
        public ICollection<TeacherGroup> TeacherGroups { get; set; } = new List<TeacherGroup>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    }
}
