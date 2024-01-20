namespace HLTV_API.Domain.Models
{
    public class UpcomingMatch
    {
        public string Url { get; set; } = null!;
        public string Event { get; set; } = null!;
        public string Type { get; set; } = null!;

        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public string TeamX { get; set; } = null!;
        public string TeamY { get; set; } = null!;
    }
}
