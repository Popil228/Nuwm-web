namespace Project1.Models.Entitys
{
    public class Group
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Course { get; set; }
        public int InstituteId { get; set; }

        public Institute? Institute { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Subgroup> Subgroups { get; set; } = new List<Subgroup>();
        public ICollection<TeacherGroup> TeacherGroups { get; set; } = new List<TeacherGroup>();

    }
}
