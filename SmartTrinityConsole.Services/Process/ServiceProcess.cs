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

namespace SmartTrinityConsole.Services.Process
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
            _messageManager.SendMsg("POST", "REQ_LAST_CONFIG_ID", "CT=DC|");
            _messageManager.SendMsg("POST", "REQ_LAST_CONFIG_ID", "REQ=REQ_FCRT_GET_GRAL_CONFIG|");

            _messageManager.SendMsg("POST", "REQ_FCRT_PUMPS_CONFIG", $"PC={Tools.GetComputerId()}|");
        }

        public object ProcessMessage(string msgType, string msgData)
        {
            _logger.LogDebug("-----------------------------------------------------------------------------------------------------------------------------------------------------");
            _logger.LogDebug($"Processing message type: {msgType} ....");
            _logger.LogDebug($"Processing data: {msgData} ....");
            _logger.LogDebug("-----------------------------------------------------------------------------------------------------------------------------------------------------");

            int pumpId = 0;
            int.TryParse(msgType.Substring(msgType.Length - 3, 3), out pumpId);

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

        public void NotifyClient(string msgData)
        {
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();

            if (data["hub"] == "PumpDeliveryProgress")
            {
                Pump pump = new Pump
                {
                    Status = "FUELLING",
                    Volume = Convert.ToDouble(data["VO"]),
                    PumpNo = Convert.ToInt32(data["pump"]),
                    SalePrice = Convert.ToDouble(data["PU"]),
                    SaleProgress = Convert.ToDouble(data["AM"]),
                    Grade = SmartPumpPersistence.GetPump(Convert.ToInt32(data["pump"])).Grade
                };
                _smartPumpHub.Clients.All.SendAsync("PumpDeliveryProgress", pump);
            }

            else if (data["hub"] == "StatusChange")
            {
                Pump pump = new Pump { PumpNo = Convert.ToInt32(data["pump"]), Status = data["ST"], SaleProgress = 0 };
                _smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);
            }
            else if (data["hub"] == "LatestPumpSales")
            {
                _smartPumpHub.Clients.All.SendAsync("LatestPumpSales", SmartSalePersistence.GetSales());
            }
            else if (data["hub"] == "UpdatePumpSales")
            {
                _smartPumpHub.Clients.All.SendAsync("UpdatePumpSales", SmartSalePersistence.GetSales());
            }
            else if (data["hub"] == "LoadPumps")
            {
                _smartPumpHub.Clients.All.SendAsync("LoadPumps", SmartPumpPersistence.GetPumps());
            }
        }

        public string ProcessMessage_EVT_PUMP_DELIVERY_PROGRESS_ID(int pumpId, string msgData)
        {
            return $"hub=PumpDeliveryProgress|pump={pumpId}|{msgData.CleanMessageData()}";
        }

        public string ProcessMessage_EVT_PUMP_STATUS_CHANGE_ID(int pumpId, string msgData)
        {
            //TODO: Create logic for change pump status...
            return $"hub=StatusChange|pump={pumpId}|{msgData.CleanMessageData()}";
        }

        public string ProcessMessage_RES_FCRT_PUMPS_CONFIG(string msgData)
        {
            //TODO: Create logic for get last pump status with: SalePrice, Volume, Sale...
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();

            int pumpQuantity = Convert.ToInt32(data["PUMPS"]);

            SmartSalePersistence.AddNewListPumpSales();

            for (int i = 1; i <= pumpQuantity; i++)
            {
                SmartPumpPersistence.AddPump(i);
                SmartSalePersistence.AddNewListPumpSales();
                _pumpProcess.CreatePump(i);
                _pumpProcess.AddPump(i);
            }

            return "hub=LoadPumps";
        }

        public string ProcessMessage_RES_FCRT_GRADES_CONFIG(string msgData)
        {
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();

            foreach (string key in data.Keys)
            {
                if (!(key.Equals("GRADES")) && key.StartsWith("G"))
                {
                    string strGrade = key.Substring(1, 3);
                    string gradeNumber = $"G{strGrade}GNR";
                    string gradeLevel = $"G{strGrade}L";
                    int gradeId;
                    int.TryParse(data[gradeNumber], out gradeId);

                    if (gradeId != 0)
                    {
                        Grade grade = SmartGradePersistence.GetGrade(gradeId);
                        if (grade == null)
                        {
                            grade = new Grade();
                            grade.Id = gradeId;
                            SmartGradePersistence.AddGrade(grade);
                        }

                        //HACK:Look for a better way to do this proccess.
                        if (key.EndsWith("DES"))
                            grade.Description = data[key];

                        else if (key.EndsWith("COL"))
                            grade.RGB = data[key];

                        else if (!key.EndsWith("LVS") && key.StartsWith(gradeLevel))
                        {
                            grade.Prices.Add(new GradePrice
                            {
                                PriceLevel = Convert.ToInt32(key.Substring(gradeLevel.Length)),
                                Price = Convert.ToDouble(data[key])
                            });
                        }

                        SmartGradePersistence.UpdateGrade(grade);
                    }
                }
            }
            return "";
        }

        public string ProcessMessage_RES_GET_PUMP_SALES(string msgData)
        {
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();
            string result = data["RC"];

            if (result.Equals("ERROR"))
            {
                _logger.LogDebug(data["MSG"]);
                return "";
            }

            int numberOfSale = Convert.ToInt32(data["QT"]);
            int pumpId = Convert.ToInt32(data["PM"]);

            List<Sale> sales = new List<Sale>();
            for (int i = 1; i <= numberOfSale; i++)
                sales.Add(CreateSaleFromMsg(data, i.ToString(), pumpId));

            sales = sales.OrderByDescending(s => s.SaleId).ToList();
            SmartSalePersistence.AddSales(pumpId, sales);

            return "hub=LatestPumpSales";
        }

        public string ProcessMessage_RES_PUMP_GET_INFO_ID(int pumpId, string msgData)
        {
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();
            Pump pump = SmartPumpPersistence.GetPump(pumpId);

            if (pump == null)
                SmartPumpPersistence.AddPump(pumpId);

            foreach (string key in data.Keys)
            {
                if (key.Equals("LV"))
                {
                    int priceLevel;
                    int.TryParse(data[key], out priceLevel);
                    pump.PriceLevel = priceLevel;
                }
                else if (key.StartsWith("H"))
                {
                    if (key.EndsWith("GR"))
                    {
                        int hoseId, gradeId;
                        int.TryParse(key.Substring(1, 1), out hoseId);
                        int.TryParse($"{data[$"H{hoseId}GR"]}", out gradeId);

                        Grade grade = SmartGradePersistence.GetGrade(gradeId);
                        if (grade != null)
                        {
                            pump.Grade = grade;
                            pump.SalePrice = SmartGradePersistence.GetSalePrice(pump.PriceLevel, grade);
                            SmartPumpPersistence.UpdatePump(pump);
                            continue;
                        }
                    }
                }
            }
            return "";
        }

        public string ProcessMessage_EVT_PUMP_NEW_TRANSACTION(string msgData)
        {
            Dictionary<string, string> data = msgData.FromMsgDataToDictionary();
            Sale sale = CreateSaleFromMsg(data, "", 0);

            IList<Sale> sales = SmartSalePersistence.GetSales()[sale.Pump];
            sales.Add(sale);
            sales = sales.OrderByDescending(s => s.SaleId).ToList();
            sales.RemoveAt(sales.Count - 1);
            SmartSalePersistence.AddSales(sale.Pump, sales);

            return $"hub=UpdatePumpSales|pump={sale.Pump}";
        }

        public Sale CreateSaleFromMsg(Dictionary<string, string> data, string strIndex, int pumpId)
        {
            Sale sale = new Sale();

            int index = 0;

            Int32.TryParse(strIndex, out index);

            if (index > 0)
                sale.Pump = pumpId;
            else
                sale.Pump = Convert.ToInt32(data[$"PM{strIndex}"]);

            sale.Date = data[$"DA{strIndex}"];
            sale.Time = data[$"TI{strIndex}"];
            sale.Hose = Convert.ToInt32(data[$"HO{strIndex}"]);
            sale.PPU = Convert.ToDouble(data[$"PU{strIndex}"]);
            sale.Type = Convert.ToInt32(data[$"TY{strIndex}"]);
            sale.SaleId = Convert.ToInt32(data[$"SA{strIndex}"]);
            sale.Volume = Convert.ToDouble(data[$"VO{strIndex}"]);
            sale.Amount = Convert.ToDouble(data[$"AM{strIndex}"]);
           sale.RGB = SmartGradePersistence.GetGrade(Convert.ToInt32(data[$"GR{strIndex}"])).RGB;

            return sale;
        }
    }
}

