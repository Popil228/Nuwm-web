namespace Project1.Models.Entitys
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Data { get; set; }

        // Foreign Key для Subgroups
        public int SubgroupId { get; set; }
        public Subgroup Subgroup { get; set; }

        // Foreign Key для Subjects
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Cabinet { get; set; }

        // Foreign Key для NumPara
        public int NumParaId { get; set; }
        public NumPara NumPara { get; set; }

        // Foreign Key для TypePara
        public int TypeParaId { get; set; }
        public TypePara TypePara { get; set; }
    }

}
