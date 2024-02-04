using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using HLTV_API.Domain.Models;
using HLTV_API.Infrastructure;

namespace HLTV_API.Application.Repositories
{
    public class LiveMatchAlertRepo : ILiveMatchAlertRepo
    {
        private readonly HltvContext _db;
        private readonly IGuildsRepo _guilds;
        public LiveMatchAlertRepo(HltvContext db, IGuildsRepo guilds)
        {
            _db = db;
            _guilds = guilds;
        }

        public async Task SetupAsync(SetupLiveMatchAlertDTO setup)
        {
            var guild = await _guilds.GetGuildAsync(setup.GuildId);
            if (guild == null)
                return;

            guild.LiveMatchAlertChannelId = setup.LiveMatchAlertChannelId;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(string guildId)
        {
            var guild = await _guilds.GetGuildAsync(guildId);
            if (guild == null)
                return;

            guild.LiveMatchAlertChannelId = null;
            await _db.SaveChangesAsync();
        }
    }
}
