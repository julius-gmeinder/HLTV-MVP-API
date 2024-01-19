using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;
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

            string htmlString = await _scraper.GetUrlAsync(new Uri(Global.BASE_URL, "/matches").AbsoluteUri, ".mainContent");
            var html = _scraper.ConvertToHtml(htmlString);

            var matchNodes = _scraper.GetChildrenByClass(html, "liveMatch");

            foreach (var matchNode in matchNodes)
            {
                if(matchNode == null) 
                    continue;

                var teams = _scraper.GetChildrenByClass(matchNode, "matchTeam");

                matches.Add(new LiveMatch()
                {
                    Url = new Uri(Global.BASE_URL, _scraper.GetChildByClass(matchNode, "match")!.GetAttributeValue("href", "")).AbsoluteUri,
                    Event = _scraper.GetChildByClass(matchNode, "matchEventName")!.InnerText,
                    Type = _scraper.GetChildByClass(matchNode, "matchMeta")!.InnerText,
                    
                    TeamX = new LiveMatchTeam()
                    {
                        Name = _scraper.GetChildByClass(teams[0]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[0]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[0]!, "mapScore")?.Element("span").InnerText
                    },

                    TeamY = new LiveMatchTeam()
                    {
                        Name = _scraper.GetChildByClass(teams[1]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[1]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[1]!, "mapScore")?.Element("span").InnerText
                    },
                });
            }

            return matches;
        }
    }
}
