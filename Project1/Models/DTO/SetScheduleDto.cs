namespace Project1.Models.DTO
{
    public class SetScheduleDto
    {
        public class TeacherPageDto
        {
            public IEnumerable<SelectItemDto>? Groups { get; set; }
            public IEnumerable<SelectItemDto>? Subgroups { get; set; }
            public IEnumerable<SelectItemDto>? PairNumbers { get; set; }
            public IEnumerable<SelectItemDto>? Types { get; set; }
            public IEnumerable<SelectItemDto>? Subjects { get; set; }
            public IEnumerable<SelectItemDto>? Teachers { get; set; }
        }

        public class SelectItemDto
        {
            public string? Value { get; set; }
            public string? Label { get; set; }
        }
    }
}
