namespace Project1.Models.Entitys
{
    public class Institute
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int GroupId { get; set; }
        public ICollection<Group> Groups { get; set; } = new List<Group>();

    }
}
