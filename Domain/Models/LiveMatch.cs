using HLTV_API.Infrastructure;
using HtmlAgilityPack;

namespace HLTV_API.Domain.Models
{
    public class LiveMatch
    {
        public class Team
        {
            public string Name { get; set; } = null!;
            public string Score { get; set; } = null!;
            public string? MapScore { get; set; } = null!;
        }

        public string Url { get; set; } = null!;
        public string Event { get; set; } = null!;
        public string Type { get; set; } = null!;

        public Team TeamX { get; set; } = null!;
        public Team TeamY { get; set; } = null!;
    }
}
