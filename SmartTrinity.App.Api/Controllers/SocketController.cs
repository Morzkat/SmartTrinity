using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Infrastructure.Communication.Adapaters.Tcp;
using SmartTrinity.Core.Services;


namespace SmartTrinity.App.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class SocketController: ControllerBase
    {
        private readonly ILogger<SocketController> _logger;
        private readonly ISmartTrinityService _smartTrinityService;

        public SocketController(ILogger<SocketController> logger, IUsersService usersService, ISmartTrinityService smartTrinityService)
        {
            _logger = logger;
            _smartTrinityService = smartTrinityService;
        }

        [HttpGet]
        public async Task<ActionResult> TestSocketConnection()
        {
            try
            {
                _smartTrinityService.Setup();
                return Ok("");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}


