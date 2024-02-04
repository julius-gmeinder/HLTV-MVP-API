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
            var newGuild = new Guild()
            {
                GuildId = setup.GuildId,
                LiveMatchAlertChannelId = setup.LiveMatchAlertChannelId
            };

            await _db.Guilds.AddAsync(newGuild);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(string guildId)
        {
            var guild = await _guilds.GetGuildAsync(guildId);
            if (guild == null)
                return;

            _db.Guilds.Remove(guild);
            await _db.SaveChangesAsync();
        }
    }
}
