using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;
using SmartTrinityConsole.Interfaces.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTrinityConsole.Services
{
    public class LongTimeTask : BackgroundService, IMainService
    {

        IPumpService _pumpService;
        ILogger<LongTimeTask> _logger;
        IServiceProcess _serviceProcess;
        IPumpProcess _pumpProcess;
        ICommunicationManager _messageManager;
        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);

        public LongTimeTask(ILogger<LongTimeTask> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService, IPumpProcess pumpProcess)
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
            /*
            _logger.LogDebug("|------------------------------------------------------|STATUS|----------------------------------------------------------------------|");
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

            else if (DateTime.Now.Minute - SmartPumpPersistence.LastUpdate.Minute > 5)
            {
                SmartPumpPersistence.LastUpdate = DateTime.Now;
                _messageManager.Client.Disconnect();
                _messageManager.Client.Connect();
                //_messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");
                _logger.LogDebug($"FROM |127.0.0.1:{Tools.CurrentSocketPort}| SENDING MESSAGE: REQ_REFRESH_GENERAL_INFORMATION DATE{SmartPumpPersistence.LastUpdate}...");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Run(() => ReadFromSocket());
                }
            }, stoppingToken);
        }
    }
}
