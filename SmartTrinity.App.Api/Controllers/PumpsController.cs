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

        /// <summary>
        /// Allow sent preset information to a pump.
        /// </summary>
        /// <param name="preset"></param>
        /// <returns>String message indicating if the preset was applied it.</returns>
        [HttpPost("Preset")]
        [ProducesResponseType(typeof(string), 200)]
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

        /// <summary>
        /// Execute a action over a pump.
        /// </summary>
        /// <param name="pumpAction"></param>
        /// <returns>String message indicating if the action was executed it.</returns>
        [HttpPost("Action")]
        [ProducesResponseType(typeof(string), 200)]
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

        /// <summary>
        /// Update the service mode of a pump
        /// </summary>
        /// <param name="pumpServiceMode"></param>
        /// <returns>String message indicating if the service mode was updated it.</returns>
        [HttpPut("ServicesModes")]
        [ProducesResponseType(typeof(string), 200)]
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
