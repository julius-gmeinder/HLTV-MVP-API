using HLTV_API.Application.Interfaces;
using HLTV_API.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HLTV_API.Controllers
{
    [Route("api/hltv/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesRepo _matches;
        public MatchesController(IMatchesRepo matches)
        {
            _matches = matches;
        }

        [HttpGet("live")]
        public async Task<IActionResult> GetLiveMatchesAsync([FromQuery] LiveMatchFilterDTO? filter)
        {
            return Ok(await _matches.GetLiveMatchesAsync(filter));
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingMatchesAsync([FromQuery] UpcomingMatchFilterDTO? filter)
        {
            return Ok(await _matches.GetUpcomingMatchesAsync(filter));
        }
    }
}
