using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpsController : ControllerBase
    {
        // IMainService _pumpProcess;
        IPumpProcess _pumpProcess;
        ILogger<PumpsController> _logger;

        public PumpsController(ILogger<PumpsController> logger, IPumpProcess pumpProcess)
        {
            _logger = logger;
            _pumpProcess = pumpProcess;
        }

        [HttpPost("Action")]
        public ActionResult<string> ExecutePumpAction([FromBody] PumpAction pumpAction)
        {
            _pumpProcess.ExecutePumpAction(pumpAction);
            return "OK";
        }

        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return "Ok";
        }
    }
}

