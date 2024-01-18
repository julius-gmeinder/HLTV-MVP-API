using HLTV_API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IWebscraperRepo _webscraperRepo;
        public MatchesController(IWebscraperRepo webscraperRepo)
        {
            _webscraperRepo = webscraperRepo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetLiveMatchesAsync()
        {
            return Ok(await _webscraperRepo.GetLiveMatchesAsync());
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestAsync()
        {
            return Ok("gulup");
        }
    }
}
