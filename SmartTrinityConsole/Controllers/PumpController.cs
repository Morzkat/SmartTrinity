using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Core.Entities.Database.Configs;
using SmartTrinityConsole.Core.Entities.Pump;
using System.Collections.Generic;
using System.Linq;

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

        [HttpPost("UpdatePumpServiceMode")]
        public ActionResult<string> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            _pumpService.UpdatePumpServiceMode(pumpServiceMode);
            return "";
        }

        [HttpPost("SendPresetToPump")]
        public ActionResult<string> SendPresetToPump(PresetConfig presetConfig)
        {
            _pumpService.SendPresent(presetConfig);
            return "";
        }
    }
}

