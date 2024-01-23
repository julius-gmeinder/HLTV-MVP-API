namespace HLTV_API.Domain.DTOs
{
    public class UpcomingMatchFilterDTO
    {
        public string? Event { get; set; } = null;
        public int? Stars { get; set; }
        public List<string>? Teams { get; set; } = null!;

        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }


        public TimeOnly? TimeFrom { get; set; }
        public TimeOnly? TimeTo { get; set; }
    }
}
