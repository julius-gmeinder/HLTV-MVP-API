using HLTV_API.Domain.Models;

namespace HLTV_API.Application.Interfaces
{
    public interface IWebscraperRepo
    {
        public Task<List<Match>> GetLiveMatchesAsync();
    }
}
