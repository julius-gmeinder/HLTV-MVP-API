using HLTV_API.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace HLTV_API.Controllers.Bot
{
    [Route("api/bot/[controller]")]
    [ApiController]
    public class GuildsController : ControllerBase
    {
        private readonly IGuildsRepo _guildsRepo;
        public GuildsController(IGuildsRepo guildsRepo)
        {
            _guildsRepo = guildsRepo;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromQuery] string guildId)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(guildId);
            if (existingGuild != null)
                return Conflict();

            await _guildsRepo.AddGuildAsync(guildId);
            return NoContent();
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string guildId)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(guildId);
            if (existingGuild == null)
                return NotFound();

            await _guildsRepo.RemoveGuildAsync(guildId);
            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] List<string> currentGuilds)
        {
            var guildsInDatabase = (await _guildsRepo.GetGuildsAsync()).Select(x => x.GuildId).ToList();

            foreach (string id in currentGuilds)
            {
                if (!guildsInDatabase.Contains(id))
                    await _guildsRepo.AddGuildAsync(id);

                guildsInDatabase.Remove(id);
            }

            foreach (string guildId in guildsInDatabase)
                await _guildsRepo.RemoveGuildAsync(guildId);

            return NoContent();
        }
    }
}
