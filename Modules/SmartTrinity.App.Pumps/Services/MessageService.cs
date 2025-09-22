using System.Reflection;
using SmartTrinity.App.Services;
using SmartTrinity.App.Extensions;
using Microsoft.AspNetCore.SignalR;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Persistence;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Core.Communication;

namespace SmartTrinity.App.Pumps.Services
{
    public class MessageService : IMessageService
    {
        private readonly IPumpsService _pumpsService;
        private readonly ICommunicationManager _communicationManager;
        private readonly IHubContext<SmartTrinityHubService, ISmartTrinityHubService> _smartTrinityHubServce;

        private const string HOSE = "H";
        private const string GRADE = "GR";
        private const string COLOR = "COL";
        private const string PRICE_LEVELS = "LVS";
        private const string HOSES = "HOSES";
        private const string DESCRIPTION = "DES";
        private const string PRICE_LEVEL = "LV";
        private const string MONEY_TOTALIZER = "MT";
        private const string VOLUME_TOTALIZER = "VT";

        public MessageService(IPumpsService pumpsService, ICommunicationManager communicationManager,
            IHubContext<SmartTrinityHubService, ISmartTrinityHubService> smartTrinityHubServce)
        {
            _pumpsService = pumpsService;
            _communicationManager = communicationManager;
            _smartTrinityHubServce = smartTrinityHubServce;
        }

        public void Process(string msgType, string msgData)
        {
            int.TryParse(msgType.Substring(msgType.Length - 3, 3), out int pumpId);
            try
            {
                MethodInfo _method = null;
                object result = null;
                string methodName = "";
                if (pumpId != 0)
                {
                    string methodName1 = $"Process_{msgType[..^4]}";
                    methodName = $"Process_{msgType.Substring(0, msgType.Length - 4)}";
                    _method = GetType().GetMethod($"{methodName}");
                    if (_method != null)
                        result = _method.Invoke(this, [pumpId, msgData]);
                }
                else
                {
                    methodName = $"Process_{msgType}";
                    _method = GetType().GetMethod($"Process_{msgType}");
                    if (_method != null)
                        result = _method.Invoke(this, [msgData]);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Process_RES_FCRT_PUMPS_CONFIG(string msgData)
        {
            var data = msgData.ToDictionary();
            int pumpsQuantity = Convert.ToInt32(data["PUMPS"]);

            for (int i = 1; i <= pumpsQuantity; i++)
            {
                PumpsPersistence.Add(i);
                _pumpsService.SetupPump(i);

                if (PumpHosesIsEmpty(i))
                    _communicationManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            }
        }

        public void Process_EVT_PUMP_STATUS_CHANGE_ID(int pumpId, string msgData)
        {
            //TODO: Create logic for change pump status...
            if (PumpHosesIsEmpty(pumpId))
                _communicationManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");

            var data = msgData.ClearMessage().ToDictionary();
            Pump pump = PumpsPersistence.Get(pumpId);
            pump.Status = data["ST"];
            Enum.TryParse(data["SU"].Split("+")[0], out PumpActions action);
            PumpsPersistence.AddAction(pumpId, action);
            pump.SaleProgress = 0;

            _smartTrinityHubServce.Clients.All.PumpStatusChangeNotification(pump);
        }

        public void Process_EVT_PUMP_DELIVERY_PROGRESS_ID(int pumpId, string msgData)
        {
            var data = msgData.ClearMessage().ToDictionary();

            Pump pump = PumpsPersistence.Get(pumpId);

            double.TryParse(data["VO"], out double volume);
            double.TryParse(data["PU"], out double salePrice);
            double.TryParse(data["AMS"], out double saleProgress);

            pump.Status = "FUELLING";
            pump.Volume = volume;
            pump.SalePrice = salePrice;
            pump.SaleProgress = saleProgress;

            _smartTrinityHubServce.Clients.All.PumpDeliveryProgressNotification(pump);
        }

        public void Process_RES_PUMP_GET_INFO_ID(int pumpId, string msgData)
        {
            var data = msgData.ToDictionary();
            var pump = PumpsPersistence.Get(pumpId);

            if (pump == null)
                PumpsPersistence.Add(pumpId);

            foreach (var key in data.Keys)
            {
                if (key.Equals(PRICE_LEVEL))
                {
                    int.TryParse(data[PRICE_LEVEL], out int priceLevel);
                    pump.PriceLevel = priceLevel;
                    continue;
                }
                else if (key.StartsWith(HOSE))
                {
                    if (key.Equals(HOSES))
                    {
                        continue;
                    }
                    else if (key.EndsWith(GRADE))
                    {
                        int.TryParse(key.Substring(1, 1), out int hoseId);
                        int.TryParse(data[HOSE + hoseId + GRADE], out int gradeId);
                        double.TryParse(data[HOSE + hoseId + MONEY_TOTALIZER], out double moneyTotalizer);
                        double.TryParse(data[HOSE + hoseId + VOLUME_TOTALIZER], out double volumeTotalizer);

                        var grade = GradesPersistence.Get(gradeId);

                        if (grade != null)
                        {
                            pump.Hoses.Add(new Hose
                            {
                                HoseId = hoseId,
                                Grade = grade,
                                TotalizerMoney = moneyTotalizer,
                                TotalizerVolume = volumeTotalizer
                            });

                        }
                    }
                }
            }
        }

        public void Process_RES_FCRT_GRADES_CONFIG(string msgData)
        {
            var data = msgData.ToDictionary();
            foreach (var key in data.Keys)
            {
                if (!key.Equals("GRADES") && key.StartsWith("G"))
                {
                    string strGrade = key.Substring(1, 3);
                    string gradeNumber = $"G{strGrade}GNR";
                    string gradeLevel = $"G{strGrade}L";
                    int.TryParse(data[gradeNumber], out int gradeId);

                    var grade = GradesPersistence.Get(gradeId);
                    if (grade == null)
                    {
                        grade = new Grade { Id = gradeId };
                        GradesPersistence.Add(grade);
                    }

                    //HACK:Look for a better way to do this proccess.
                    if (key.EndsWith(DESCRIPTION))
                        grade.Description = data[key];

                    else if (key.StartsWith(gradeLevel) && !key.EndsWith(PRICE_LEVELS))
                    {
                        int.TryParse(key.Substring(gradeLevel.Length), out int priceLevel);
                        double.TryParse(data[key], out double price);
                    }

                    GradesPersistence.Update(grade);
                }
            }
        }

        private bool PumpHosesIsEmpty(int pumpId)
        {
            Pump pump = PumpsPersistence.Get(pumpId);
            if (!pump.Hoses.Any())
                return true;

            return false;
        }
    }
}
