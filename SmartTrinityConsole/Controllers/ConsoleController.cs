using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityApi.Hubs;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        // IMainService _mainService;
        IMemoryCache _memoryCache;
        ILogger<ConsoleController> _logger;
        IHubContext<SmartPumpHub> _hub;

        public ConsoleController(ILogger<ConsoleController> logger, IMemoryCache memoryCache, IHubContext<SmartPumpHub> hub)
        {
            _hub = hub;
            _logger = logger;
            // _mainService = mainService;
            _memoryCache = memoryCache;
        }

        [HttpGet("ConnectToServer")]
        public ActionResult<string> ConnectToServer()
        {
            // _mainService.ReadFromSocketContinuously();
            return "OK";
        }

        [HttpGet]
        public ActionResult<string> Get() 
        {   
           _hub.Clients.All.SendAsync("PumpDeliveryProgress", "....");

            return "Request Completed";        
        }

    }
}

