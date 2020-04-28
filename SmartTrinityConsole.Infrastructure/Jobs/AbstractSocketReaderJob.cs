using System;
using SmartTrinityApi.Common;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Infrastructure.Jobs
{
    public abstract class AbstractSocketReaderJob
    {
        protected IPumpService _pumpService;
        protected IServiceProcess _serviceProcess;
        protected ICommunicationManager _messageManager;
        protected ILogger<AbstractSocketReaderJob> _logger;

        public AbstractSocketReaderJob(ILogger<AbstractSocketReaderJob> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService)
        {
            _logger = logger;

            _pumpService = pumpService;
            _serviceProcess = serviceProcess;
            _messageManager = messageManager;

            SmartPumpPersistence.LastUpdate = DateTime.Now;
        }

        public virtual void ReadFromSocket()
        {
            if (_messageManager.SocketHasData())
            {
                _messageManager.ReceiveSubscribedMessages();
                _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                SmartPumpPersistence.LastUpdate = DateTime.Now;
            }

            else if (!_messageManager.ClientIsConnected())
            {
                SmartUserPersistence.LastReply = "";
                SmartPumpPersistence.LastUpdate = DateTime.Now;
                _messageManager.Connect();
                _pumpService.PumpsBaseConfig();
            }

            else if (Math.Abs(DateTime.Now.Minute - SmartPumpPersistence.LastUpdate.Minute) > 5)
            {
                _messageManager.Disconnect();
                SmartPumpPersistence.LastUpdate = DateTime.Now;
                SmartUserPersistence.LastReply = "";
                // _messageManager.Connect();
                // _pumpService.PumpsBaseConfig();
            }
        }
    }
}