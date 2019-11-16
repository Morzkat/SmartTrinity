using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpController : ControllerBase
    {
        // IMainService _mainService;
        IMainService _mainService;
        ILogger<PumpController> _logger;

        public PumpController(ILogger<PumpController> logger, IMainService mainService)
        {
            _logger = logger;
            _mainService = mainService;
        }

        [HttpPost("ClosePump")]
        public ActionResult<string> ClosePump([FromBody] PumpAction pumpAction)
        {
            _mainService.ExecutePumpAction(pumpAction);
            return "OK";
        }

        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return "Ok";
        }
    }
}

