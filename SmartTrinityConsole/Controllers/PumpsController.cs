using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Core.ServerResponse;
using SmartTrinityConsole.Core.Entities.Sale;

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
        public ActionResult<Response> ExecutePumpAction([FromBody] PumpAction pumpAction)
        {
            return _pumpService.ExecutePumpAction(pumpAction);
        }

        [HttpGet("ServicesModes")]
        public ActionResult<ResponseWithList<PumpServiceMode>> GetPumpsAndServicesModes()
        {
            return _pumpService.GetPumpsAndServicesModes();
        }

        //TODO: Create logic for get last sales from the controller.
        [HttpGet("{pumpNo}/Sales")]
        public ActionResult<ResponseWithList<Sale>> GetSales(int pumpNo)
        {
            return _pumpService.GetSales(pumpNo);
        }

        [HttpPut("ServicesModes")]
        public ActionResult<Response> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            return _pumpService.UpdatePumpServiceMode(pumpServiceMode);
        }

        [HttpPost("Send/Preset")]
        public ActionResult<Response> SendPresetToPump(PresetConfig presetConfig)
        {
            return _pumpService.SendPresent(presetConfig);
        }
    }
}

