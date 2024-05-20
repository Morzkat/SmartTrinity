using SmartTrinity.App.Api.Extensions;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Persistence;

namespace SmartTrinity.App.Api.Core
{

    public static class Events
    {
        public const string
            SESSION = "SESSION",
            SUBSCRIBE = "SUBSCRIBE",
            SUBSCRIBE_ALL = "ALL",
            SubscribeLastConfigId = "EVT_NEW_CONFIG_APPLIED",
            PumpStatusChangeId = "EVT_PUMP_STATUS_CHANGE_ID_*",
            NewConfigApplied = "EVT_NEW_CONFIG_APPLIED",
            PumpDeliveryProgress = "EVT_PUMP_DELIVERY_PROGRESS_ID_",
            PumpTotalizerUpdateId = "EVT_PUMP_TOTALIZER_UPDATE_ID_",
            PumpNewTransaction = "EVT_PUMP_NEW_TRANSACTION",
            PumpPriceLevelId = "EVT_PUMP_PRICE_LEVEL_ID_*",
            GradePriceChange = "EVT_GRADE_PRICE_CHANGE",
            EntryPumpController = "EVT_ENTRY_PUMP_CONTROLLER",
            ExitPumpController = "EVT_EXIT_PUMP_CONTROLLER",
            EntryClientConnection = "EVT_ENTRY_CLIENT_CONNECTION",
            ExitClientConnection = "EVT_EXIT_CLIENT_CONNECTION",
            ShiftPeriodClosed = "EVT_SHIFT_PERIOD_CLOSED",
            TktPeriodClosed = "EVT_TKT_PERIOD_CLOSED_",
            PaymentSaleCleared = "EVT_PAYMENT_SALE_CLEARED",
            PaymentSaleWarningOn = "EVT_PAYMENT_SALE_WARNING_ON",
            PaymentSaleWarningOff = "EVT_PAYMENT_SALE_WARNING_OFF",
            PaymentPumpWarningOff = "EVT_PAYMENT_SALE_WARNING_OFF",
            PaymentPumpWarningON = "EVT_PAYMENT_PUMP_WARNING_ON",
            PaymentAtributesChange = "EVT_PAYMENT_ATTRIBUTES_CHANGE",
            PumpErrorMsgId = "EVT_PUMP_ERROR_MSG_ID_",
            TktTrxNewId = "EVT_TKT_TRX_NEW_ID_",
            TktTrxUpdateId = "EVT_TKT_TRX_UPDATE_ID_",
            TktPeriodClose = "EVT_TKT_PERIOD_CLOSE";
    }

    public interface INotificationManager
    {
        //
        void NotifyClients(string eventType, Notification notification);
    }

    public class Notification
    {
        public string Endpoint { get; set; }
        public int PumpId { get; set; }
        public string Data { get; set; }
    }

    public class NotificationManager : INotificationManager
    {
        public Dictionary<string, Func<Notification, int>> _notifiers { get; set; }

        public NotificationManager()
        {
            _notifiers.Add("", NotifyEventPumpDeliveryProgressById);
        }

        private int NotifyEventPumpDeliveryProgressById(Notification notification)
        {
            var pumpProgress = notification.Data.ClearMessage().ToDictionary();

            double.TryParse(pumpProgress["VO"], out double volume);
            double.TryParse(pumpProgress["PU"], out double salePrice);
            double.TryParse(pumpProgress["AMS"], out double saleProgress);

            Pump pump = PumpsPersistence.GetPump(notification.PumpId);

            //TODO: Create a enum with pump status type
            pump.Status = "FUELLING";
            pump.Volume = volume;
            pump.PumpNo = notification.PumpId;
            pump.SalePrice = salePrice;
            pump.SaleProgress = saleProgress;
            pump.Grade = PumpsPersistence.GetPump(notification.PumpId).Grade;

            return 0;
        }

        public int NotifyEventPumpStatusChangeId(Notification notification)
        {
            ////TODO: Create logic for change pump status...
            //if (PumpHosesIsEmpty(pumpId))
            //{
            //    _messageManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            //}

            Dictionary<string, string> data = notification.Data.ClearMessage().ToDictionary();
            //Pump pump = SmartPumpPersistence.GetPump(pumpId);
            //pump.Status = data["ST"];
            //SmartPumpPersistence.AddActionToPump(pumpId.ToString(), data["SU"].Split("+")[0]);
            //SmartPumpPersistence.UpdatePump(pump);

            //return new NotifyPumpChangesToClient($"pump={pumpId}|{msgData.CleanMessageData()}".FromMsgDataToDictionary());

            return 0;
        }

        public int ResponseFcrtPumpsConfig(Notification notification)
        {
            Dictionary<string, string> data = notification.Data.ToDictionary();

            int pumpQuantity = Convert.ToInt32(data["PUMPS"]);

            //for (int i = 1; i <= pumpQuantity; i++)
            //{
            //    SmartPumpPersistence.AddPump(i);
            //    _pumpProcess.CreatePump(i);
            //    _pumpProcess.AddPump(i);

            //    if (PumpHosesIsEmpty(i))
            //    {
            //        _messageManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            //    }
            //}

            //return new NotifyPumpsToClient();

            return 0;
        }

        public int ResponseFcrtGradesConfig(Notification notification)
        {
            Dictionary<string, string> data = notification.Data.ToDictionary();

            //foreach (string key in data.Keys)
            //{
            //    if (!(key.Equals("GRADES")) && key.StartsWith("G"))
            //    {
            //        string strGrade = key.Substring(1, 3);
            //        string gradeNumber = $"G{strGrade}GNR";
            //        string gradeLevel = $"G{strGrade}L";
            //        int gradeId;
            //        int.TryParse(data[gradeNumber], out gradeId);

            //        if (gradeId != 0)
            //        {
            //            Grade grade = SmartGradePersistence.GetGrade(gradeId);
            //            if (grade == null)
            //            {
            //                grade = new Grade();
            //                grade.Id = gradeId;
            //                SmartGradePersistence.AddGrade(grade);
            //            }

            //            //HACK:Look for a better way to do this proccess.
            //            if (key.EndsWith("DES"))
            //                grade.Description = data[key];

            //            else if (key.EndsWith("COL"))
            //                grade.RGB = data[key];

            //            else if (!key.EndsWith("LVS") && key.StartsWith(gradeLevel))
            //            {
            //                grade.Prices.Add(new GradePrice
            //                {
            //                    PriceLevel = Convert.ToInt32(key.Substring(gradeLevel.Length)),
            //                    Price = Convert.ToDouble(data[key])
            //                });
            //            }

            //            SmartGradePersistence.UpdateGrade(grade);
            //        }
            //    }
            //}
            //return new NoNotificationToClient();
            return 0;
        }

        //public int ResponseGetPumpSales(Notification notification)
        //{
        //    Dictionary<string, string> data = notification.Data.ToDictionary();
        //    string result = data["RC"];

        //    if (result.Equals("ERROR"))
        //    {
        //        _logger.LogError(data["MSG"]);
        //        return new NotifyLatestPumpSalesToClient();
        //    }

        //    int numberOfSale = Convert.ToInt32(data["QT"]);
        //    int pumpId = Convert.ToInt32(data["PM"]);

        //    List<Sale> sales = new List<Sale>();
        //    for (int i = 1; i <= numberOfSale; i++)
        //        sales.Add(CreateSaleFromMsg(data, i.ToString(), pumpId));

        //    sales = sales.OrderByDescending(s => s.SaleId).ToList();
        //    SmartSalePersistence.AddSales(pumpId, sales);

        //    return new NotifyLatestPumpSalesToClient();
        //}

        public int ResponseGetPumpInfoById(Notification notification)
        {
            Dictionary<string, string> data = notification.Data.ToDictionary();
            //Pump pump = SmartPumpPersistence.GetPump(pumpId);

            //if (pump == null)
            //    SmartPumpPersistence.AddPump(pumpId);

            //foreach (string key in data.Keys)
            //{
            //    if (key.Equals("LV"))
            //    {
            //        int priceLevel;
            //        int.TryParse(data[key], out priceLevel);
            //        pump.PriceLevel = priceLevel;
            //        continue;
            //    }
            //    else if (key.StartsWith("H"))
            //    {
            //        if (key.Equals("HOSES"))
            //        {
            //            int.TryParse(data[key], out int hoseId);
            //            SmartHosePersistence.AddHose(hoseId);
            //            continue;
            //        }
            //        if (key.EndsWith("GR"))
            //        {
            //            int.TryParse(key.Substring(1, 1), out int hoseId);
            //            double.TryParse(data["H" + hoseId + "MT"], out double totalizerMoney);
            //            double.TryParse(data["H" + hoseId + "VT"], out double totalizerVolume);

            //            Hose hose = SmartHosePersistence.GetHose(hoseId);

            //            int.TryParse($"{data[$"H{hoseId}GR"]}", out int gradeId);
            //            Grade grade = SmartGradePersistence.GetGrade(gradeId);

            //            if (grade != null)
            //            {
            //                bool exist = hose.Grades.Any(g => g.Description == grade.Description);

            //                if (!exist)
            //                    hose.Grades.Add(grade);

            //                hose.TotalizerMoney = totalizerMoney;
            //                hose.TotalizerVolume = totalizerVolume;
            //                SmartHosePersistence.UpdateHose(hose);

            //                pump.Grade = grade;
            //                pump.Hoses.Add(hose);
            //                pump.SalePrice = SmartGradePersistence.GetSalePrice(pump.PriceLevel, grade);
            //                SmartPumpPersistence.UpdatePump(pump);

            //                continue;
            //            }
            //        }
            //    }
            //}
            //return new NotifyPumpsToClient();

            return 0;
        }

        public int EventPumpNewTransaction(Notification notification)
        {
            //Dictionary<string, string> data = msgData.FromMsgDataToDictionary();
            //Sale sale = CreateSaleFromMsg(data, "", 0);

            //IList<Sale> sales = SmartSalePersistence.GetSales()[sale.Pump];
            //sales.Add(sale);
            //sales = sales.OrderByDescending(s => s.SaleId).ToList();
            //sales.RemoveAt(sales.Count - 1);
            //SmartSalePersistence.AddSales(sale.Pump, sales);

            //return new NotifyLatestPumpSalesToClient($"pump={sale.Pump}".FromMsgDataToDictionary());

            return 0;
        }

        public void NotifyClients(string eventType, Notification notification)
        {
            throw new NotImplementedException();
        }

        private bool PumpHosesIsEmpty(int pumpId)
        {
            Pump pump = PumpsPersistence.GetPump(pumpId);
            if (!pump.Hoses.Any())
                return true;

            return false;
        }
    }
}
