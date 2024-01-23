using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using HtmlAgilityPack;

namespace HLTV_API.Application.Repositories
{
    public class MatchesRepo : IMatchesRepo
    {
        private readonly Webscraper _scraper;

        public MatchesRepo(Webscraper scraper)
        {
            _scraper = scraper;
        }

        public async Task<List<LiveMatch>> GetLiveMatchesAsync(LiveMatchFilterDTO? filter)
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
                    Url = GetMatchUrl(matchNode),
                    Event = GetMatchEvent(matchNode),
                    Type = GetMatchType(matchNode),
                    Stars = GetMatchStars(matchNode),

                    TeamX = GetLiveMatchTeam(teams[0]!),
                    TeamY = GetLiveMatchTeam(teams[1]!)
                });
            }

            return LiveMatchesFilter(matches, filter);
        }

        public async Task<List<UpcomingMatch>> GetUpcomingMatchesAsync(UpcomingMatchFilterDTO? filter)
        {
            List<UpcomingMatch> matches = new();

            string htmlString = await _scraper.GetUrlAsync(new Uri(Global.BASE_URL, "/matches").AbsoluteUri, ".mainContent");
            var html = _scraper.ConvertToHtml(htmlString);

            var upcomingMatchesNodes = _scraper.GetChildrenByClass(html, "upcomingMatchesSection");

            foreach (var upcomingMatchesNode in upcomingMatchesNodes)
            {
                if (upcomingMatchesNode == null)
                    continue;

                var date = GetUpcomingMatchDate(upcomingMatchesNode);

                var matchNodes = _scraper.GetChildrenByClass(upcomingMatchesNode, "upcomingMatch");

                foreach (var matchNode in matchNodes)
                {
                    if (matchNode == null)
                        continue;

                    var teamNameNodes = _scraper.GetChildrenByClass(matchNode, "matchTeamName").Select(x => x!.InnerText).ToList();
                    if (teamNameNodes.Count < 2)
                        continue;

                    matches.Add(new UpcomingMatch()
                    {
                        Url = GetMatchUrl(matchNode),
                        Event = GetMatchEvent(matchNode),
                        Type = GetMatchType(matchNode),
                        Stars = GetMatchStars(matchNode),

                        TeamX = teamNameNodes[0]!,
                        TeamY = teamNameNodes[1]!,

                        Date = date,
                        Time = GetUpcomingMatchTime(matchNode)
                    });
                }
            }
            return UpcomingMatchesFilter(matches, filter);
        }

        private List<LiveMatch> LiveMatchesFilter(List<LiveMatch> matches, LiveMatchFilterDTO? filter)
        {
            if (filter == null)
                return matches;

            if (filter.Stars != null)
                matches = matches.Where(x => x.Stars >= filter.Stars).ToList();

            if (filter.Event != null)
                matches = matches.Where(x => x.Event.ToLower().Contains(filter.Event.ToLower())).ToList();

            if (filter.Teams != null && filter.Teams.Count > 0)
                matches = matches.Where(match => {
                    foreach (var team in filter.Teams.ConvertAll(team => team.ToLower()))
                        if (match.TeamX.Name.ToLower() == team || match.TeamY.Name.ToLower() == team)
                            return true;
                    return false;
                }).ToList();

            return matches;
        }

        private List<UpcomingMatch> UpcomingMatchesFilter(List<UpcomingMatch> matches, UpcomingMatchFilterDTO? filter)
        {
            if (filter == null)
                return matches;

            if (filter.Stars != null)
                matches = matches.Where(x => x.Stars >= filter.Stars).ToList();

            if (filter.Event != null)
                matches = matches.Where(x => x.Event.ToLower().Contains(filter.Event.ToLower())).ToList();

            if (filter.Teams != null && filter.Teams.Count > 0)
                matches = matches.Where(match => {
                        foreach (var team in filter.Teams.ConvertAll(team => team.ToLower()))
                            if (match.TeamX.ToLower() == team || match.TeamY.ToLower() == team)
                                return true;
                        return false;
                }).ToList();

            if (filter.From != null && filter.To != null)
            {
                matches = matches.Where(match => {
                        DateTime matchDateTime = new DateTime(match.Date.Year, match.Date.Month, match.Date.Day, match.Time.Hour, match.Time.Minute, match.Time.Second);

                        if(filter.From <= matchDateTime && matchDateTime <= filter.To)
                            return true;
                        return false;
                }).ToList();
            }

            return matches;
        }

        private string GetMatchUrl(HtmlNode node)
        {
            return new Uri(Global.BASE_URL, _scraper.GetChildByClass(node, "match")!.GetAttributeValue("href", "")).AbsoluteUri;
        }

        private string GetMatchEvent(HtmlNode node)
        {
            return _scraper.GetChildByClass(node, "matchEventName")!.InnerText;
        }

        private string GetMatchType(HtmlNode node)
        {
            return _scraper.GetChildByClass(node, "matchMeta")!.InnerText;
        }

        private int GetMatchStars(HtmlNode node)
        {
            var starsNode = _scraper.GetChildByClass(node, "matchRating");
            int starsCount = Convert.ToInt32(5 - _scraper.GetChildrenByClass(starsNode!, "faded").Count);
            return starsCount;
        }

        private LiveMatch.Team GetLiveMatchTeam(HtmlNode node)
        {
            return new LiveMatch.Team()
            {
                Name = _scraper.GetChildByClass(node!, "matchTeamName")!.InnerText,
                Score = Convert.ToInt32(_scraper.GetChildByClass(node!, "currentMapScore")!.InnerText.Trim()),
                MapScore = Convert.ToInt32(_scraper.GetChildByClass(node!, "mapScore")?.Element("span").InnerText)
            };
        }

        private DateOnly GetUpcomingMatchDate(HtmlNode node)
        {
            var dateNode = _scraper.GetChildByClass(node, "matchDayHeadline");
            string dateString = dateNode!.InnerText.Substring(dateNode.InnerText.Length - 10, 10);
            return DateOnly.ParseExact(dateString, "yyyy-MM-dd", null);
        }

        private TimeOnly GetUpcomingMatchTime(HtmlNode node)
        {
            return TimeOnly.Parse(_scraper.GetChildByClass(node, "matchTime")!.InnerText);
        }
    }
}
