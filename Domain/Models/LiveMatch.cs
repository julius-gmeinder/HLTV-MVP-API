using HLTV_API.Infrastructure;
using HtmlAgilityPack;

namespace HLTV_API.Domain.Models
{
    public class LiveMatch
    {
        public class Team
        {
            public string Name { get; set; } = null!;
            public int? Score { get; set; }
            public int? MapScore { get; set; }
        }

        public string Url { get; set; } = null!;
        public string Event { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Stars { get; set; }

        public Team TeamX { get; set; } = null!;
        public Team TeamY { get; set; } = null!;
    }
}
