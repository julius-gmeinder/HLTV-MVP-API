using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HLTV_API.Application.Repositories
{
    public class GuildsRepo : IGuildsRepo
    {
        private readonly HltvContext _db;
        public GuildsRepo(HltvContext db)
        {
            _db = db;
        }

        public async Task<Guild?> GetGuildAsync(string guildId)
        {
            return await _db.Guilds.FirstOrDefaultAsync(x => x.GuildId == guildId);
        }

        public async Task AddAsync(string guildId)
        {
            await _db.Guilds.AddAsync(new Guild()
            {
                GuildId = guildId
            });
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(string guildId)
        {
            var existingGuild = await GetGuildAsync(guildId);
            if (existingGuild == null)
                return;

            _db.Guilds.Remove(existingGuild);
            await _db.SaveChangesAsync();
        }
    }
}
