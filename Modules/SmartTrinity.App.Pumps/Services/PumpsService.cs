using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp.Extensions;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Pumps.Infrastructure;
using SmartTrinity.App.Pumps.Persistence;

namespace SmartTrinity.App.Pumps.Services
{
    public class PumpsService : IPumpsService
    {
        IPumpsUnitOfWork _pumpsUnitOfWork;
        ISmartTrinityService _smartTrinityService;
        ICommunicationManager _communicationManager;

        public PumpsService(IPumpsUnitOfWork pumpsUnitOfWork, ICommunicationManager communicationManager, ISmartTrinityService smartTrinityService)
        {
            _pumpsUnitOfWork = pumpsUnitOfWork;
            _smartTrinityService = smartTrinityService;
            _communicationManager = communicationManager;

            if (!_communicationManager.ClientIsConnected())
                _smartTrinityService.Setup();
        }

        public async Task<string> ExecuteAction(PumpAction pumpAction)
        {
            try
            {
                SetPumpStatus(pumpAction.Pump, pumpAction.Action);

                var p = PumpsPersistence.GetPumpAction(pumpAction.Pump.ToString());
                if (PumpsPersistence.GetPumpAction(pumpAction.Pump.ToString()) == "MONEY_PRESET" || PumpsPersistence.GetPumpAction(pumpAction.Pump.ToString()) == "VOLUME_PRESET")
                    _communicationManager.SendMsg("POST", $"REQ_PUMP_CLEAR_PRESET_ID_0{pumpAction.Pump.ToString().PadLeft(2, '0')}", "");
                else
                    _communicationManager.SendMsg("POST", $"REQ_PUMP_{pumpAction.Action.ToString().ToUpper()}_ID_0{pumpAction.Pump.ToString().PadLeft(2, '0')}", "");

                return "Accion ejecutada sobre el lado.";
                //_smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);
            }
            catch (Exception e)
            {
                //_logger.LogError($"Error executing pump action: {e.Message}");
                return $"Error ejecutando la accion {pumpAction.Action} sobre el lado {pumpAction.Pump}.";
            }
        }

        private void SetPumpStatus(int pumpId, PumpActions action)
        {
            Pump pump = PumpsPersistence.GetPump(pumpId);
            pump.SetAthoredStatus(action);
            PumpsPersistence.UpdatePump(pump);
        }

        public async Task<string> SendPresent(Preset preset)
        {
            try
            {
                string eventType = $"REQ_PUMP_PRESET_ID_{preset.PumpNo.ToString().LPad("0", 3)}";
                string data = "TY=" + (preset.Type == 1 ? "MONEY" : "VOLUME");

                if (preset.TankFull)
                    data += $"|VA=FULL";
                else
                    data += $"|VA={preset.Amount}";

                if (preset.Grades != null)
                {
                    string grades = "";

                    foreach (var grade in preset.Grades)
                    {
                        if (grades.Equals(""))
                            grades = $"{grade.Id}";
                        else
                            grades += $",{grade.Id}";
                    }

                    data += $"|GR={grades}";
                }

                data += "|";

                _communicationManager.SendMsg("POST", eventType, data);

                return "El 'Preset' fue enviado al lado.";
            }
            catch (Exception e)
            {
                //_logger.LogError($"ERROR sending 'Preset' to pump {preset.PumpNo}: {e.Message}");
                return $"Error enviando el 'Preset' al lado {preset.PumpNo}.";
            }
        }

        public async Task<string> UpdateServiceMode(ServiceMode pumpServiceMode)
        {
            try
            {
                _pumpsUnitOfWork.Begin();
                bool serviceModesUpdated = await _pumpsUnitOfWork.GenericConfigValuesRepository.UpdatePumpServiceMode(pumpServiceMode);

                if (serviceModesUpdated)
                {
                    _pumpsUnitOfWork.Commit();
                    _communicationManager.SendMsg("POST", "REQ_APPLY_GRAL_NEW_CONFIG", "");
                    _communicationManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");

                    return "El modo de servicio ha sido cambiado";
                }
                else
                {
                    //_logger.LogError($"Error in the transaction, check database transaction for more details.");
                    _pumpsUnitOfWork.Rollback();
                    return "Error cambiando el modo de servicio del lado";
                }
            }
            catch (Exception e)
            {
                //_logger.LogError($"Error updating pump service mode: {e.Message}");
                return $"Error cambiando el modo de servicio del lado.";
            }
        }
    }
}
