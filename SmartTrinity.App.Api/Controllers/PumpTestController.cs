using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartTrinity.App.Pumps.Core.Services;

namespace SmartTrinity.App.Api.Controllers
{

    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/Pumps/Test")]
    public class PumpTestController : ControllerBase
    {
        private readonly IPumpsTestService _pumpsService;
        private readonly ILogger<PumpTestController> _logger;

        public PumpTestController(ILogger<PumpTestController> logger, IPumpsTestService pumpsService)
        {
            _logger = logger;
            _pumpsService = pumpsService;
        }

        [HttpPost("Preset/{pumpNo}")]
        public async Task<ActionResult> SendTestPresent(int pumpNo)
        {
            try
            {
                return Ok(await _pumpsService.SendTestPresent(pumpNo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Preset/stop/{pumpNo}")]
        public async Task<ActionResult> StopTestPresent(int pumpNo)
        {
            try
            {
                return Ok(await _pumpsService.SendTestPresent(pumpNo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
