using HLTV_API.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddAsync(string guildId)
        {
            await _guildsRepo.AddAsync(guildId);
            return NoContent();
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAsync(string guildId)
        {
            await _guildsRepo.RemoveAsync(guildId);
            return NoContent();
        }
    }
}
