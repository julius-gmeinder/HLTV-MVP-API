using HLTV_API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesRepo _matches;
        public MatchesController(IMatchesRepo matches)
        {
            _matches = matches;
        }

        [HttpGet("live")]
        public async Task<IActionResult> GetLiveMatchesAsync()
        {
            return Ok(await _matches.GetLiveMatchesAsync());
        }
    }
}
