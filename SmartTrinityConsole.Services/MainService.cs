using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class MainService : IMainService
    {
        ILogger<MainService> _logger;
        IServiceProcess _serviceProcess;
        IPumpService _pumpService;
        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);
        ICommunicationManager _messageManager;

        public MainService(ILogger<MainService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService)
        {
            _logger = logger;
            _pumpService = pumpService;
            _messageManager = messageManager;
            _serviceProcess = serviceProcess;
            _messageManager.ConnectionParams = connectionParams;

            _messageManager.Connect();
            _pumpService.PumpsBaseConfig();

        }

        public void ReadFromSocketContinuously()
        {
            Task pumpSellingProcessTask = new Task(() =>
            {
                while (true)
                {
                    if (_messageManager.SocketHasData())
                    {
                        _messageManager.ReceiveSubscribedMessages();
                        _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                    }
                }
            });
            pumpSellingProcessTask.Start();
        }
    }
}
