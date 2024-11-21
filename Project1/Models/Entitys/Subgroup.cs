namespace Project1.Models.Entitys
{
    public class Subgroup
    {

        public int Id { get; set; }
        public int Number { get; set; }
        public int GroupId { get; set; }

        public Group? Group { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
