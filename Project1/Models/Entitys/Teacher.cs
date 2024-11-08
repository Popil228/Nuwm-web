namespace Project1.Models.Entitys
{
    public class Teacher
    {

        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public ICollection<TeacherGroup> TeacherGroups { get; set; } = new List<TeacherGroup>();

    }
}
