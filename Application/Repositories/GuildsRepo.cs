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
    }
}
