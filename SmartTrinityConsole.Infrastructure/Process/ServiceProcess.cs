using SmartTrinityApi.Common;
using Microsoft.Extensions.Logging;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using System;

namespace SmartTrinityConsole.Infrastructure.Process
{
    public class ServiceProcess : IServiceProcess
    {
        IPumpProcess _pumpProcess;
        ILogger<ServiceProcess> _logger;
        ICommunicationManager _messageManager;

        public ServiceProcess(ILogger<ServiceProcess> logger, ICommunicationManager messageManager, IPumpProcess pumpProcess)
        {
            _logger = logger;
            _pumpProcess = pumpProcess;
            _messageManager = messageManager;
        }

        public void AddPumpSalesProccess()
        {
            _messageManager.Subscribe("EVT_NEW_CONFIG_APPLIED");
            _messageManager.Subscribe("EVT_ENTRY_PUMP_CONTROLLER");
            _messageManager.Subscribe("EVT_EXIT_PUMP_CONTROLLER");
            _messageManager.Subscribe("EVT_PUMP_ERROR_MSG_ID_*");
            _messageManager.Subscribe("EVT_PUMP_STATUS_CHANGE_ID_*");
            _messageManager.Subscribe("EVT_PUMP_NEW_TRANSACTION");
            _messageManager.Subscribe("EVT_PUMP_PRICE_LEVEL_ID_*");
            _messageManager.Subscribe("EVT_GRADE_PRICE_CHANGE");
            _messageManager.Subscribe("EVT_PUMP_DELIVERY_PROGRESS_ID_*");
            _messageManager.Subscribe("EVT_PUMP_TOTALIZER_UPDATE_ID_*");
        }

        public void ProccessStationData()
        {
            _messageManager.SendMsg("POST", "REQ_FCRT_GET_GRAL_CONFIG", "");
            _messageManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            _messageManager.SendMsg("POST", "REQ_LAST_CONFIG_ID", "");
            _messageManager.SendMsg("POST", "REQ_FCRT_PUMPS_CONFIG", $"PC={Tools.GetComputerId()}|");
        }

        public bool CheckConfig()
        {

            return false;
        }

        public bool ProcessMessage(string msgType, string msgData) 
        {
            _logger.LogDebug($"Processing message type {msgType} ....");
            if (msgType.Equals("RES_FCRT_PUMPS_CONFIG"))
            {
                ProcessMessage_RES_FCRT_PUMPS_CONFIG(msgData);
            } 
            else if(msgType.StartsWith("EVT_PUMP_DELIVERY_PROGRESS_ID_"))
            {
                ProcessMessage_EVT_PUMP_DELIVERY_PROGRESS_ID(msgData);
            }
            return true;
        }

        private void ProcessMessage_EVT_PUMP_DELIVERY_PROGRESS_ID(string msgData)
        {
            string evt = "EVT_PUMP_DELIVERY_PROGRESS_ID_";
            // int pumpId = Convert.ToInt32(evt.Substring(evt.Length, evt.Length + 3));
            _logger.LogDebug($"Data: {msgData}");
        }

        private void ProcessMessage_RES_FCRT_PUMPS_CONFIG(string msgData)
        {
            string[] data = msgData.Split("|");

            int pumpQuantity = Convert.ToInt32(data[3].Substring(6, 1));

            for (int i = 1; i <= pumpQuantity; i++)
            {
                _pumpProcess.CreatePump(i);
                _pumpProcess.AddPump(i);
            }
        }

    }
}

