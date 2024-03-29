﻿using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HLTV_API.Controllers.Bot
{
    [Route("api/bot/[controller]")]
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

        [HttpGet("")]
        public async Task<IActionResult> GetChannelsAsync()
        {
            return Ok(await _liveAlerts.GetChannelsAsync());
        }

        [HttpPatch("setup")]
        public async Task<IActionResult> SetupAsync([FromBody] SetupLiveMatchAlertDTO setupDTO)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(setupDTO.GuildId);
            if (existingGuild == null)
                return NotFound();

            if (existingGuild.LiveMatchAlertChannelId != null)
                return Conflict();

            await _liveAlerts.SetupAsync(setupDTO);
            return NoContent();
        }

        [HttpPatch("remove")]
        public async Task<IActionResult> RemoveAsync([FromQuery] string guildId)
        {
            var existingGuild = await _guildsRepo.GetGuildAsync(guildId);
            if (existingGuild == null)
                return NotFound();

            await _liveAlerts.RemoveAsync(guildId);
            return NoContent();
        }
    }
}
