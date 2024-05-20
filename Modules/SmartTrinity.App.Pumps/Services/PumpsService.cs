using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Pumps.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Services
{
    public class PumpsService : IPumpsService
    {

        public PumpsService(IPumpsUnitOfWork pumpsUnitOfWork)
        {
            
        }

        //public Result<Response> UpdateServiceMode(PumpServiceMode pumpServiceMode)
        //{
        //    try
        //    {
        //        _unitOfWork.Begin();
        //        bool successfull_transaction = _unitOfWork.GenericConfigValuesRepository.UpdatePumpServiceMode(pumpServiceMode);

        //        if (successfull_transaction)
        //        {
        //            _unitOfWork.Commit();
        //            _messageManager.SendMsg("POST", "REQ_APPLY_GRAL_NEW_CONFIG", "");
        //            _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");

        //            return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("El modo de servicio ha sido cambiado", success: true));
        //        }
        //        else
        //        {
        //            _unitOfWork.Rollback();
        //            _logger.LogError($"Error in the transaction, check database transaction for more details.");
        //            return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("Error cambiando el modo de servicio del lado", "Error in the transaction, check database transaction.", true));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"Error updating pump service mode: {e.Message}");
        //        return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error cambiando el modo de servicio del lado.", $"ERROR: {e.Message}"));
        //    }
        //}

        //public Result<Response> ExecutePumpAction(PumpAction pumpAction)
        //{
        //    try
        //    {
        //        Pump pump = SmartPumpPersistence.GetPump(pumpAction.Pump);
        //        pump.SetAthoredStatus(pumpAction.Action);
        //        SmartPumpPersistence.UpdatePump(pump);

        //        _smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);

        //        _pumpProcess.ExecutePumpAction(pumpAction);
        //        return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("Accion ejecutada sobre el lado.", success: true));
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"Error executing pump action: {e.Message}");
        //        return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error ejecutando la accion {pumpAction.Action} sobre el lado {pumpAction.Pump}.", $"ERROR executing pump action: {e.Message}"));
        //    }
        //}

        //public Result<Response> SendPresent(PresetConfig presetConfig)
        //{
        //    try
        //    {
        //        _userService.LogInUser();

        //        string eventType = $"REQ_PUMP_PRESET_ID_{Tools.LPad(presetConfig.PumpNo.ToString(), "0", 3)}";
        //        string data = "TY=" + (presetConfig.Type == 1 ? "MONEY" : "VOLUME");

        //        if (presetConfig.TankFull)
        //            data += $"|VA=FULL";
        //        else
        //            data += $"|VA={presetConfig.Amount}";

        //        if (presetConfig.Grades != null)
        //        {
        //            string grades = "";

        //            foreach (var grade in presetConfig.Grades)
        //            {
        //                if (grades.Equals(""))
        //                    grades = $"{grade.Id}";
        //                else
        //                    grades += $",{grade.Id}";
        //            }

        //            data += $"|GR={grades}";
        //        }

        //        data += "|";

        //        _messageManager.SendMsg("POST", eventType, data);

        //        if (_userService.UserIsConnected())
        //            return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("El 'Preset' fue enviado al lado.", success: true));

        //        //TODO: parameterize this error...
        //        return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse(error: "Ocurrio un error enviando el preset."));
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"ERROR sending 'Preset' to pump {presetConfig.PumpNo}: {e.Message}");
        //        return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error enviando el 'Preset' al lado {presetConfig.PumpNo}.", $"ERROR sending 'Preset' to pump: {e.Message}"));
        //    }
        //}
        public Task ExecuteAction(PumpAction pumpAction)
        {
            var pump = new Pump();
            if (pumpAction.Action.ToString() != null) { }
           // Pump pump = SmartPumpPersistence.GetPump(pumpAction.Pump);
            //pump.SetAthoredStatus(pumpAction.Action);

            //if (SmartPumpPersistence.GetPumpAction(pumpAction.Pump.ToString()) == "MONEY_PRESET" || SmartPumpPersistence.GetPumpAction(pumpAction.Pump.ToString()) == "VOLUME_PRESET")
            //    _messageManager.SendMsg("POST", $"REQ_PUMP_CLEAR_PRESET_ID_0{pumpAction.Pump.ToString().PadLeft(2, '0')}", "");
            //else
            //    _messageManager.SendMsg("POST", $"REQ_PUMP_{pumpAction.Action.ToUpper()}_ID_0{pumpAction.Pump.ToString().PadLeft(2, '0')}", "");
         
            throw new NotImplementedException();
        }

        public Task SendPresent(Preset preset)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateServiceMode(ServiceMode pumpServiceMode)
        {
            throw new NotImplementedException();
        }
    }
}
