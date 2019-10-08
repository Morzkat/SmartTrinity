using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Services.Tcp.Communication;
using SmartTrinityConsole.Services.CommunicationManager;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        ILogger<ConsoleController> _logger;

        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);
        ICommunicationManager _messageManager;

        public ConsoleController(ILogger<ConsoleController> logger, ICommunicationManager messageManager)
        {
            _logger = logger;
            _messageManager = messageManager;

            _messageManager.SetConnectionParams(connectionParams);
        }

        [HttpGet("ConnectToServer")]
        public ActionResult<string> ConnectToServer()
        {
            _messageManager.Connect();
            return "OK";
        }

        [HttpGet("SendMessage")]
        public ActionResult<string> SendMessage()
        {
            _logger.LogDebug("Sending EVT");
            _messageManager.SendMsg("SUBSCRIBE", "EVT_PAYMENT_SALE_WARNING_ON", "");
            return "";
        }


    }
}