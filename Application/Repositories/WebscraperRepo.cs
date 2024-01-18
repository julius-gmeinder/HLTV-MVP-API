using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HLTV_API.Application.Repositories
{
    public class WebscraperRepo : IWebscraperRepo
    {
        private readonly Webscraper _scraper;
        public WebscraperRepo(Webscraper scraper)
        {
            _scraper = scraper;
        }
        public async Task<List<Match>> GetLiveMatchesAsync()
        {
            string htmlString = await _scraper.GetUrlAsync("https://www.hltv.org/matches", ".liveMatch-container");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            var liveMatches = htmlDoc.DocumentNode.Descendants(0).Where(x => x.HasClass("liveMatch"));

            List<Match> matches = new List<Match>();
            foreach (var liveMatch in liveMatches)
            {
                matches.Add(new Match
                {
                    Length = liveMatch.Descendants(0).FirstOrDefault(x => x.HasClass("matchMeta")).InnerText,
                    TeamOne = liveMatch.Descendants(0).FirstOrDefault(x => x.HasClass("matchTeamName")).InnerText,
                    TeamOneScore = Convert.ToInt32(liveMatch.Descendants(0).FirstOrDefault(x => x.HasClass("currentMapScore")).InnerText),
                    TeamTwo = liveMatch.Descendants(0).LastOrDefault(x => x.HasClass("matchTeamName")).InnerText,
                    TeamTwoScore = Convert.ToInt32(liveMatch.Descendants(0).LastOrDefault(x => x.HasClass("currentMapScore")).InnerText),
                });
            }

            return matches;
        }
    }
}
