using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HLTV_API.Application.Repositories
{
    public class MatchesRepo : IMatchesRepo
    {
        private readonly Webscraper _scraper;
        public MatchesRepo(Webscraper scraper)
        {
            _scraper = scraper;
        }

        public async Task<List<LiveMatch>> GetLiveMatchesAsync()
        {
            List<LiveMatch> matches = new();

            string htmlString = await _scraper.GetUrlAsync("https://www.hltv.org/matches", ".mainContent");
            var html = _scraper.ConvertToHtml(htmlString);

            var liveMatches = _scraper.GetChildrenByClass(html, "liveMatch");

            foreach (var liveMatch in liveMatches)
            {
                if(liveMatch == null) 
                    continue;

                var teams = _scraper.GetChildrenByClass(liveMatch, "matchTeam");

                matches.Add(new LiveMatch()
                {
                    Event = _scraper.GetChildByClass(liveMatch, "matchEventName")!.InnerText,
                    Type = _scraper.GetChildByClass(liveMatch, "matchMeta")!.InnerText,
                    
                    TeamX = new LiveMatchTeam()
                    {
                        Name = _scraper.GetChildByClass(teams[0]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[0]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[0]!, "mapScore")?
                                           .Descendants(0)
                                           .FirstOrDefault(x => x.HasClass("leading") || x.HasClass("trailing"))?.InnerText
                    },

                    TeamY = new LiveMatchTeam()
                    {
                        Name = _scraper.GetChildByClass(teams[1]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[1]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[1]!, "mapScore")?
                                           .Descendants(0)
                                           .FirstOrDefault(x => x.HasClass("leading") || x.HasClass("trailing"))?.InnerText
                    },
                });
            }

            return matches;
        }
    }
}
