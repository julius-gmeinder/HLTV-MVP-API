namespace HLTV_API.Domain.DTOs
{
    public class LiveMatchFilterDTO
    {
        public string? Event { get; set; } = null;
        public int? Stars { get; set; }
        public List<string>? Teams { get; set; } = null!;
    }
}
