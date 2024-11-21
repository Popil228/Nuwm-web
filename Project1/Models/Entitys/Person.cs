using Microsoft.AspNetCore.Identity;

namespace Project1.Models.Entitys
{
    public class Person : IdentityUser
    {

        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? ThirdName { get; set; }

        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }

    }
}
