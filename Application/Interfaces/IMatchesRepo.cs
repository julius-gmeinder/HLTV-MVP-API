using HLTV_API.Domain.Models;

namespace HLTV_API.Application.Interfaces
{
    public interface IMatchesRepo
    {
        public Task<List<LiveMatch>> GetLiveMatchesAsync();
    }
}
