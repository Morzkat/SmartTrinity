using System.Linq;
using System.Threading;
using SmartTrinityApi.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class PumpService : IPumpService
    {
        IUnitOfWork _unitOfWork;
        IPumpProcess _pumpProcess;
        ILogger<PumpService> _logger;
        IServiceProcess _serviceProcess;
        ICommunicationManager _messageManager;

        public PumpService(ILogger<PumpService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IUnitOfWork unitOfWork, IPumpProcess pumpProcess)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pumpProcess = pumpProcess;
            _serviceProcess = serviceProcess;
            _messageManager = messageManager;
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

                catch { break; };
            }
        }

        //TODO: Create logic for change "Pump Service Mode" and notify the server
        public void UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            _unitOfWork.Begin();
            _unitOfWork.GenericConfigValuesRepository.UpdatePumpServiceMode(pumpServiceMode);
            _unitOfWork.Commit();
            _messageManager.SendMsg("POST", "REQ_APPLY_GRAL_NEW_CONFIG", "");
            _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");

        }

        public void ExecutePumpAction(PumpAction pumpAction)
        {
            _pumpProcess.ExecutePumpAction(pumpAction);
        }

        public List<PumpServiceMode> GetPumpsAndServicesModes()
        {
            return _unitOfWork.ConfigValuesRepository.GetPumpsAndServicesModes().ToList();
        }
        /**
            type = MONEY
            type = VOLUME
        **/


        public void SendPresent(PresetConfig presetConfig)
        {
            string eventType = $"REQ_PUMP_PRESET_ID_{Tools.LPad(presetConfig.PumpNo.ToString(), "0", 3)}";
            string data = "TY=" + (presetConfig.Type == 1 ? "MONEY" : "VOLUME");

            if (presetConfig.TankFull)
                data += $"|VA=FULL";
            else
                data += $"|VA={presetConfig.Amount}";

            if (presetConfig.Grades != null)
            {
                string grades = "";

                foreach (var grade in presetConfig.Grades)
                {
                    if (grades.Equals(""))
                        grades = $"{grade.Id}";
                    else
                        grades += $",{grade.Id}";
                }

                data += $"|GR={grades}|";
            }
            _messageManager.SendMsg("POST", eventType, data);
        }
    }
}

