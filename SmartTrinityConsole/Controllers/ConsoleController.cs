using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;
using System.Threading;
using SmartTrinityConsole.Services.Sales;
using SmartTrinityConsole.Infrastructure.Process;
using SmartTrinityApi.Core.Interfaces.Process;

namespace SmartTrinityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        ILogger<ConsoleController> _logger;
        IServiceProcess _serviceProcess;

        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);
        ICommunicationManager _messageManager;

        public ConsoleController(ILogger<ConsoleController> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess)
        {
            _logger = logger;
            _messageManager = messageManager;
            _serviceProcess = serviceProcess;
            _messageManager.ConnectionParams = connectionParams;
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
            //TODO: Move logic to other class (create default controller for add all necesaries events)...
            _serviceProcess.ProccessStationData();
            while (Thread.CurrentThread.IsAlive)
            {
                try
                {

                    _messageManager.ReceiveSubscribedMessages();
                    _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);

                    if (_messageManager.MsgType.Equals("RES_FCRT_PUMPS_CONFIG"))
                    {
                        break;
                    }
                }

                catch (Exception e)
                {
                    _logger.LogError($"Error: {e.Message}");
                }
            }

            _serviceProcess.AddPumpSalesProccess();
            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    _messageManager.ReceiveSubscribedMessages();
                    _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                    //_messageManager.SendMsg("ECHO", "ECHO", "SPIRIT");
                }

                catch (Exception e)
                {
                    _logger.LogError($"Error: {e.Message}");
                }
            }
            return "";
        }
    }
}