using HLTV_API.Infrastructure;
using HtmlAgilityPack;

namespace HLTV_API.Domain.Models
{
    public class LiveMatch
    {
        public string Url { get; set; } = null!;
        public string Event { get; set; } = null!;
        public string Type { get; set; } = null!;

        public LiveMatchTeam TeamX { get; set; } = null!;
        public LiveMatchTeam TeamY { get; set; } = null!;
    }
}
