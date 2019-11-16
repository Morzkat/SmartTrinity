using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        // IMainService _mainService;
        ILogger<ConsoleController> _logger;

        public ConsoleController(ILogger<ConsoleController> logger)
        {
            _logger = logger;
        }

        /*
        [HttpGet("ConnectToServer")]
        public ActionResult<string> ConnectToServer()
        {
            _mainService.ReadFromSocket();
            return "OK";
        }
        */
        [HttpGet("Test")]
        public ActionResult<IList<IList<Sale>>> Test()
        {
            return new List<IList<Sale>>();
        }
    }
}

