using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Services;

namespace SmartTrinity.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class PumpsController : ControllerBase
    {
        private readonly IPumpsService _pumpsService;
        private readonly ILogger<PumpsController> _logger;

        public PumpsController(ILogger<PumpsController> logger, IPumpsService pumpsService)
        {
            _logger = logger;
            _pumpsService = pumpsService;
        }

        [HttpPost("Send/Preset")]
        public async Task<ActionResult> SendPreset(Preset preset)
        {
            try
            {
                return Ok(await _pumpsService.SendPresent(preset));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Action")]
        public async Task<ActionResult> ExecuteAction([FromBody] PumpAction pumpAction)
        {
            try
            {
                return Ok(await _pumpsService.ExecuteAction(pumpAction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ServicesModes")]
        public async Task<ActionResult> UpdateServiceMode(ServiceMode pumpServiceMode)
        {
            try
            {
                return Ok(await _pumpsService.UpdateServiceMode(pumpServiceMode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
