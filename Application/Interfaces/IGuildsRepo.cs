using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;

namespace HLTV_API.Application.Interfaces
{
    public interface IGuildsRepo
    {
        public Task<List<Guild>> GetGuildsAsync();
        public Task<Guild?> GetGuildAsync(string GuildId);
        public Task AddGuildAsync(string guildId);
        public Task RemoveGuildAsync(string guildId);
    }
}
