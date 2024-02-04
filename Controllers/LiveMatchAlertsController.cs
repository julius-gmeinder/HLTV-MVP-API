using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveMatchAlertsController : ControllerBase
    {
        private readonly IGuildsRepo _guildsRepo;
        private readonly ILiveMatchAlertRepo _liveAlerts;
        public LiveMatchAlertsController(IGuildsRepo guildsRepo, ILiveMatchAlertRepo liveAlerts)
        {
            _guildsRepo = guildsRepo;
            _liveAlerts = liveAlerts;
        }

        [HttpPost("add")]
        public async Task<IActionResult> SetupLiveMatchAlertAsync(SetupLiveMatchAlertDTO setupDTO)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(setupDTO.GuildId);
            if (existingGuild != null)
                return Conflict();

            await _liveAlerts.SetupAsync(setupDTO);
            return NoContent();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveLiveMatchAlertSetupAsync(string guildId)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(guildId);
            if (existingGuild == null)
                return NotFound();

            await _liveAlerts.RemoveAsync(guildId);
            return NoContent();
        }
    }
}
