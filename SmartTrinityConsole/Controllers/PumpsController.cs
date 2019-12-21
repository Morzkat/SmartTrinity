using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Services;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpsController : ControllerBase
    {
        IPumpService _pumpService;
        ILogger<PumpsController> _logger;

        public PumpsController(ILogger<PumpsController> logger, IPumpService pumpService)
        {
            _logger = logger;
            _pumpService = pumpService;
        }

        [HttpPost("Action")]
        public ActionResult<string> ExecutePumpAction([FromBody] PumpAction pumpAction)
        {
            _pumpService.ExecutePumpAction(pumpAction);
            return "OK";
        }

        [HttpGet("ServicesModes")]
        public ActionResult<List<PumpServiceMode>> GetPumpsAndServicesModes()
        {
            return _pumpService.GetPumpsAndServicesModes();
        }

        [HttpPut("ServiceMode")]
        public ActionResult<string> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            _pumpService.UpdatePumpServiceMode(pumpServiceMode);
            return "";
        }

        [HttpPost("PresetToPump")]
        public ActionResult<string> SendPresetToPump(PresetConfig presetConfig)
        {
            _pumpService.SendPresent(presetConfig);
            return "";
        }
    }
}

