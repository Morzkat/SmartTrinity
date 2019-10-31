using System;
using System.Linq;
using System.Reflection;
using SmartTrinityApi.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Infrastructure.Hubs;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Common.ExtensionMethods.String;

namespace SmartTrinityConsole.Infrastructure.Process
{
    public class ServiceProcess : IServiceProcess
    {
        IPumpProcess _pumpProcess;
        ILogger<ServiceProcess> _logger;
        ICommunicationManager _messageManager;
        IHubContext<SmartPumpHub> _smartPumpHub;

        public ServiceProcess(ILogger<ServiceProcess> logger, ICommunicationManager messageManager, IPumpProcess pumpProcess, IHubContext<SmartPumpHub> smartPumpHub)
        {
            _logger = logger;
            _pumpProcess = pumpProcess;
            _smartPumpHub = smartPumpHub;
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

        public object ProcessMessage(string msgType, string msgData)
        {
            _logger.LogDebug("-----------------------------------------------------------------------------------------------------------------------------------------------------");
            _logger.LogDebug($"Processing message type: {msgType} ....");
            _logger.LogDebug($"Processing data: {msgData} ....");
            _logger.LogDebug("-----------------------------------------------------------------------------------------------------------------------------------------------------");

            int pumpId = 0;
            Int32.TryParse(msgType.Substring(msgType.Length - 3, 3), out pumpId);

            try
            {
                MethodInfo _method = null;
                object result = null;
                string methodName = "";
                if (pumpId != 0)
                {
                    methodName = $"ProcessMessage_{msgType.Substring(0, msgType.Length - 4)}";
                    _method = this.GetType().GetMethod($"{methodName}");
                    result = _method.Invoke(this, new object[] { pumpId, msgData });
                    ProcessMessageResponse(result.ToString());
                }
                //TODO: Remove else (because the method will return objects...)
                else
                {
                    methodName = $"ProcessMessage_{msgType}";
                    _method = this.GetType().GetMethod($"ProcessMessage_{msgType}");
                    result = _method.Invoke(this, new object[] { msgData });
                    ProcessMessageResponse(result.ToString());
                }
            }
            catch { _logger.LogInformation($"Process not found {msgType}"); }

            return true;
        }

        public void ProcessMessageResponse(string data)
        {
            if (data == "")
            {
                return;
            }
            NotifyClient(data);
        }

        public void NotifyClient(string data)
        {
            Dictionary<string, string> processedData = data.FromMsgDataToDictionary();

            if (processedData["hub"] == "PumpDeliveryProgress")
            {
                Pump pump = new Pump
                {
                    Status = "FUELLING",
                    Volume = Convert.ToDouble(processedData["VO"]),
                    PumpNo = Convert.ToInt32(processedData["pump"]),
                    SalePrice = Convert.ToDouble(processedData["PU"]),
                    SaleProgress = Convert.ToDouble(processedData["AM"])
                };

                _smartPumpHub.Clients.All.SendAsync("PumpDeliveryProgress", pump);
            }

            else if (processedData["hub"] == "StatusChange")
            {
                Pump pump = new Pump { PumpNo = Convert.ToInt32(processedData["pump"]), Status = processedData["ST"], SaleProgress = 0 };
                _smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);
            }
            else if (processedData["hub"] == "LatestPumpSales")
            {
                _smartPumpHub.Clients.All.SendAsync("LatestPumpSales", SmartPersistence.GetSales());
            }
            else if (processedData["hub"] == "UpdatePumpSales")
            {
                _smartPumpHub.Clients.All.SendAsync("UpdatePumpSales", SmartPersistence.GetSales());
            }
            else if (processedData["hub"] == "LoadPumps")
            {
                _smartPumpHub.Clients.All.SendAsync("LoadPumps", SmartPersistence.GetPumps());
            }
        }

        public string ProcessMessage_EVT_PUMP_DELIVERY_PROGRESS_ID(int pumpId, string msgData)
        {
            return $"hub=PumpDeliveryProgress|pump={pumpId}|{msgData.CleanMessageData()}";
        }

        public string ProcessMessage_EVT_PUMP_STATUS_CHANGE_ID(int pumpId, string msgData)
        {
            return $"hub=StatusChange|pump={pumpId}|{msgData.CleanMessageData()}";
        }

        public string ProcessMessage_RES_FCRT_PUMPS_CONFIG(string msgData)
        {
            string[] data = msgData.Split("|");

            int pumpQuantity = Convert.ToInt32(data[3].Substring(6, 1));

            SmartPersistence.AddNewListPumpSales();

            for (int i = 1; i <= pumpQuantity; i++)
            {
                SmartPersistence.AddPump(i);
                SmartPersistence.AddNewListPumpSales();
                _pumpProcess.CreatePump(i);
                _pumpProcess.AddPump(i);
            }
            return "hub=LoadPumps";
        }

        public string ProcessMessage_RES_GET_PUMP_SALES(string msgData)
        {
            Dictionary<string, string> processedData = msgData.FromMsgDataToDictionary();
            string result = processedData["RC"];

            if (result.Equals("ERROR"))
            {
                _logger.LogDebug(processedData["MSG"]);
                return "";
            }

            int numberOfSale = Convert.ToInt32(processedData["QT"]);
            int pumpId = Convert.ToInt32(processedData["PM"]);

            List<Sale> sales = new List<Sale>();
            for (int i = 1; i <= numberOfSale; i++)
                sales.Add(CreateSaleFromMsg(processedData, i.ToString(), pumpId));

            sales = sales.OrderByDescending(s => s.SaleId).ToList();
            SmartPersistence.AddSales(pumpId, sales);

            return "hub=LatestPumpSales";
        }

        public string ProcessMessage_EVT_PUMP_NEW_TRANSACTION(string msgData)
        {
            Dictionary<string, string> processedData = msgData.FromMsgDataToDictionary();
            Sale sale = CreateSaleFromMsg(processedData, "", 0);

            IList<Sale> sales = SmartPersistence.GetSales()[sale.Pump];
            sales.Add(sale);
            sales = sales.OrderByDescending(s => s.SaleId).ToList();
            sales.RemoveAt(sales.Count - 1);
            SmartPersistence.AddSales(sale.Pump, sales);
            return $"hub=UpdatePumpSales|pump={sale.Pump}";
        }

        public Sale CreateSaleFromMsg(Dictionary<string, string> processedData, string strIndex, int pumpId)
        {
            Sale sale = new Sale();

            int index = 0;

            Int32.TryParse(strIndex, out index);

            if (index > 0)
                sale.Pump = pumpId;
            else
                sale.Pump = Convert.ToInt32(processedData[$"PM{strIndex}"]);

            sale.SaleId = Convert.ToInt32(processedData[$"SA{strIndex}"]);
            sale.Volume = Convert.ToDouble(processedData[$"VO{strIndex}"]);
            sale.Amount = Convert.ToDouble(processedData[$"AM{strIndex}"]);
            sale.Hose = Convert.ToInt32(processedData[$"HO{strIndex}"]);
            sale.PPU = Convert.ToDouble(processedData[$"PU{strIndex}"]);
            sale.Type = Convert.ToInt32(processedData[$"TY{strIndex}"]);
            sale.Date = processedData[$"DA{strIndex}"];
            sale.Time = processedData[$"TI{strIndex}"];

            return sale;
        }
    }
}

