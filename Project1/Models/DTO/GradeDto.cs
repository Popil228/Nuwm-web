namespace Project1.Models.DTO
{
    public class GradeDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public float Value { get; set; }
    }

    public class GradeDeleteDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
