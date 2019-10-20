using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using SmartTrinityApi.Core.Interfaces.Services;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        IMainService _mainService;
        IMemoryCache _memoryCache;
        ILogger<ConsoleController> _logger;
        IHubContext<PumpSalesHub> _hub;

        public ConsoleController(ILogger<ConsoleController> logger, IMemoryCache memoryCache, IMainService mainService, IHubContext<PumpSalesHub> hub)
        {
            _hub = hub;
            _logger = logger;
            _mainService = mainService;
            _memoryCache = memoryCache;
        }

        [HttpGet("ConnectToServer")]
        public ActionResult<string> ConnectToServer()
        {
            _mainService.ReadFromSocketContinuously();
            return "OK";
        }

        [HttpGet]
        public ActionResult<string> Get() 
        {   
           _hub.Clients.All.SendAsync("ReceiveMessage", "....");

            return "Request Completed";        
        }

    }
}

