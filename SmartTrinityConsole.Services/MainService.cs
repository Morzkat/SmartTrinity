using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class MainService
    {
        IPumpService _pumpService;
        private IMemoryCache _cache;
        ILogger<MainService> _logger;
        IServiceProcess _serviceProcess;
        IPumpProcess _pumpProcess;
        ICommunicationManager _messageManager;
        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);

        public MainService(ILogger<MainService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService, IPumpProcess pumpProcess)
        {
            _logger = logger;
            _pumpProcess = pumpProcess;
            _pumpService = pumpService;
            _messageManager = messageManager;
            _serviceProcess = serviceProcess;
            _messageManager.ConnectionParams = connectionParams;

            _messageManager.Connect();
            _pumpService.PumpsBaseConfig();

        }

        public string ExecutePumpAction(PumpAction pumpAction)
        {
            _pumpProcess.ExecutePumpAction(pumpAction);
            return "Ok";
        }

        public void ReadFromSocket()
        {
            SmartPumpPersistence.LastUpdate = DateTime.Now;
            Task pumpSellingProcessTask = new Task(() =>
            {
                while (true)
                {
                    /* _logger.LogDebug("|------------------------------------------------------|STATUS|----------------------------------------------------------------------|");
                     _logger.LogDebug($"Current time {DateTime.Now}| last update: |{SmartPumpPersistence.LastUpdate}|");
                     _logger.LogDebug($"Socket is connected: {_messageManager.IsConnected}");
                     _logger.LogDebug("|------------------------------------------------------|END STATUS|------------------------------------------------------------------|");
                    */
                    if (_messageManager.SocketHasData())
                    {
                        SmartPumpPersistence.LastUpdate = DateTime.Now;
                        _messageManager.ReceiveSubscribedMessages();
                        _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                    }
                    else if (!_messageManager.Client.IsConnected())
                        _messageManager.Client.Connect();

                    else if (DateTime.Now.Minute - SmartPumpPersistence.LastUpdate.Minute > 3)
                    {
                        SmartPumpPersistence.LastUpdate = DateTime.Now;
                        _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");
                        _logger.LogDebug($"FROM |127.0.0.1:{Tools.CurrentSocketPort}| SENDING MESSAGE: REQ_REFRESH_GENERAL_INFORMATION...");
                    }
                }
            });
            pumpSellingProcessTask.Start();
        }




    }
}
