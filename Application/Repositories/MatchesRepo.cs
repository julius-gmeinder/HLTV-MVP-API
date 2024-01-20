using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
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
                    
                    TeamX = new LiveMatch.Team()
                    {
                        Name = _scraper.GetChildByClass(teams[0]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[0]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[0]!, "mapScore")?.Element("span").InnerText
                    },

                    TeamY = new LiveMatch.Team()
                    {
                        Name = _scraper.GetChildByClass(teams[1]!, "matchTeamName")!.InnerText,
                        Score = _scraper.GetChildByClass(teams[1]!, "currentMapScore")!.InnerText.Trim(),
                        MapScore = _scraper.GetChildByClass(teams[1]!, "mapScore")?.Element("span").InnerText
                    },
                });
            }

            return matches;
        }

        public async Task<List<UpcomingMatch>> GetUpcomingMatchesAsync()
        {
            List<UpcomingMatch> matches = new();

            string htmlString = await _scraper.GetUrlAsync(new Uri(Global.BASE_URL, "/matches").AbsoluteUri, ".mainContent");
            var html = _scraper.ConvertToHtml(htmlString);

            var upcomingMatchesNodes = _scraper.GetChildrenByClass(html, "upcomingMatchesSection");

            foreach (var upcomingMatchesNode in upcomingMatchesNodes)
            {
                if (upcomingMatchesNode == null)
                    continue;

                var dateNode = _scraper.GetChildByClass(upcomingMatchesNode, "matchDayHeadline");
                string dateString = dateNode!.InnerText.Substring(dateNode.InnerText.Length - 10, 10);
                var matchDate = DateOnly.ParseExact(dateString, "yyyy-MM-dd", null);

                var matchNodes = _scraper.GetChildrenByClass(upcomingMatchesNode, "upcomingMatch");
                foreach (var matchNode in matchNodes)
                {
                    if (matchNode == null)
                        continue;

                    var teamNameNodes = _scraper.GetChildrenByClass(matchNode, "matchTeamName");
                    if (teamNameNodes.Count < 2)
                        continue;

                    var matchTime = TimeOnly.Parse(_scraper.GetChildByClass(matchNode, "matchTime")!.InnerText);

                    matches.Add(new UpcomingMatch()
                    {
                        Url = new Uri(Global.BASE_URL, _scraper.GetChildByClass(matchNode, "match")!.GetAttributeValue("href", "")).AbsoluteUri,
                        Event = _scraper.GetChildByClass(matchNode, "matchEventName")!.InnerText,
                        Type = _scraper.GetChildByClass(matchNode, "matchMeta")!.InnerText,
                        Date = matchDate,
                        Time = matchTime,
                        TeamX = teamNameNodes[0]!.InnerText,
                        TeamY = teamNameNodes[1]!.InnerText,
                    });
                }
            }
            return matches;
        }
    }
}
