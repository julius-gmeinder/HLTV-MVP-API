namespace HLTV_API.Domain.DTOs
{
    public class UpcomingMatchFilterDTO
    {
        public string? Event { get; set; } = null;
        public int? Stars { get; set; }
        public List<string>? Teams { get; set; } = null!;

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
