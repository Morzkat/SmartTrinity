using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        // IMainService _mainService;
        ILogger<ConsoleController> _logger;
        IUnitOfWork _unitOfWork;
        public ConsoleController(ILogger<ConsoleController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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
            var p = _unitOfWork.ConfigValuesRepository.Get(2);
            return new List<IList<Sale>>();
        }
    }
}

