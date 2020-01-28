using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Core.Entities.ServerResponse;
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
        public ActionResult<Result<Response>> ExecutePumpAction([FromBody] PumpAction pumpAction)
        {
            Result<Response> result = _pumpService.ExecutePumpAction(pumpAction);
            return StatusCode(result.StatusCode, result.Response);
        }

        [HttpGet("ServicesModes")]
        public ActionResult<Result<ResponseWithList<PumpServiceMode>>> GetPumpsAndServicesModes()
        {
            Result<ResponseWithList<PumpServiceMode>> result = _pumpService.GetPumpsAndServicesModes();
            return StatusCode(result.StatusCode, result.Response);
        }

        [HttpGet("{pumpNo}/Sales")]
        public ActionResult<Result<ResponseWithList<Sale>>> GetSales(int pumpNo)
        {
            Result<ResponseWithList<Sale>> result = _pumpService.GetSales(pumpNo);
            return StatusCode(result.StatusCode, result.Response); 
        }

        [HttpPut("ServicesModes")]
        public ActionResult<Result<Response>> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            Result<Response> result = _pumpService.UpdatePumpServiceMode(pumpServiceMode);
            return StatusCode(result.StatusCode, result.Response); 
        }

        [HttpPost("Send/Preset")]
        public ActionResult<Result<Response>> SendPresetToPump(PresetConfig presetConfig)
        {
            Result<Response> result = _pumpService.SendPresent(presetConfig); 
            return StatusCode(result.StatusCode, result.Response); 
        }
    }
}

