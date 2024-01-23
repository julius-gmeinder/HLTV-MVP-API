using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;

namespace HLTV_API.Application.Interfaces
{
    public interface IMatchesRepo
    {
        public Task<List<LiveMatch>> GetLiveMatchesAsync(LiveMatchFilterDTO? filter);
        public Task<List<UpcomingMatch>> GetUpcomingMatchesAsync(UpcomingMatchFilterDTO? filter);
    }
}
