namespace Project1.Models.Entitys
{
    public class Student
    {

        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int? SubgroupId { get; set; }
        public Subgroup? Subgroup { get; set; }

    }
}
