using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SmartTrinity.App.Extensions;
using SmartTrinity.App.Pumps.Persistence;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Services;

namespace SmartTrinity.App.Pumps.Services
{
    public class MessageService : IMessageService
    {
        private readonly IPumpsService _pumpsService;
        private readonly ICommunicationManager _communicationManager;

        public MessageService(IPumpsService pumpsService, ICommunicationManager communicationManager)
        {
            _pumpsService = pumpsService;
            _communicationManager = communicationManager;
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
            Dictionary<string, string> data = msgData.ToDictionary();
            int pumpsQuantity = Convert.ToInt32(data["PUMPS"]);

            for (int i = 1; i <= pumpsQuantity; i++)
            {
                PumpsPersistence.Add(i);
                _pumpsService.SetupPump(i);

                if (PumpHosesIsEmpty(i))
                    _communicationManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            }

            // return new NotifyPumpsToClient();
        }

        public void Process_EVT_PUMP_STATUS_CHANGE_ID(int pumpId, string msgData)
        {
            //TODO: Create logic for change pump status...
            if (PumpHosesIsEmpty(pumpId))
                _communicationManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");

            Dictionary<string, string> data = msgData.ClearMessage().ToDictionary();
            Pump pump = PumpsPersistence.Get(pumpId);
            pump.Status = data["ST"];
            Enum.TryParse(data["SU"].Split("+")[0], out PumpActions action);
            PumpsPersistence.AddAction(pumpId, action);
            PumpsPersistence.Update(pump);

            // return new NotifyPumpChangesToClient($"pump={pumpId}|{msgData.CleanMessageData()}".FromMsgDataToDictionary());
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
