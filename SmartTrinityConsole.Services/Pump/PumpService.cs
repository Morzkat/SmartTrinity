using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Interfaces.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SmartTrinityConsole.Services.Pump
{
    public class PumpService : IPumpService
    {
        ILogger<PumpService> _logger;
        IServiceProcess _serviceProcess;
        ICommunicationManager _messageManager;

        public PumpService(ILogger<PumpService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess)
        {
            _logger = logger;
            _serviceProcess = serviceProcess;
            _messageManager = messageManager;
        }

        public int PumpSalesProcess()
        {
            return 0;
        }

        public void PumpsBaseConfig()
        {
            _serviceProcess.ProccessStationData();
            while (Thread.CurrentThread.IsAlive)
            {
                _messageManager.ReceiveSubscribedMessages();
                _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);

                if (_messageManager.MsgType.Equals("RES_FCRT_PUMPS_CONFIG"))
                    break;
            }

            _serviceProcess.AddPumpSalesProccess();

            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    _messageManager.ReceiveSubscribedMessages();
                    _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                }

                catch (Exception e)
                {
                    break;
                };
            }
        }
    }
}

