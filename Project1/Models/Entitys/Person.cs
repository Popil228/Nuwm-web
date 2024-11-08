namespace Project1.Models.Entitys
{
    public class Person
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? ThirdName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }

    }
}
