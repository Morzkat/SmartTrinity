using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartTrinity.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class PumpsController: ControllerBase
    {
        private readonly ILogger<PumpsController> _logger;

        public PumpsController(ILogger<PumpsController> logger)
        {
            _logger = logger;  
        }


    }
}
