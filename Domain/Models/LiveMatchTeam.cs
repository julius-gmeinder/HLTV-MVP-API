namespace HLTV_API.Domain.Models
{
    public class LiveMatchTeam
    {
        public string Name { get; set; } = null!;
        public string Score { get; set; } = null!;
        public string? MapScore { get; set; } = null!;
    }
}
