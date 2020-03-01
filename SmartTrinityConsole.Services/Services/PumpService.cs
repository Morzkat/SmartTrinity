using System;
using System.Linq;
using System.Threading;
using SmartTrinityApi.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Core.Entities.ServerResponse;
using SmartTrinityApi.Core.Entities.ServerResponse.Helpers;
using Microsoft.AspNetCore.SignalR;
using SmartTrinityApi.Infrastructure.Hubs;

namespace SmartTrinityConsole.Services
{
    public class PumpService : IPumpService
    {
        IUnitOfWork _unitOfWork;
        IUserService _userService;
        IPumpProcess _pumpProcess;
        ILogger<PumpService> _logger;
        IServiceProcess _serviceProcess;
        ICommunicationManager _messageManager;
        IHubContext<SmartPumpHub> _smartPumpHub;

        public PumpService(ILogger<PumpService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IUnitOfWork unitOfWork, IPumpProcess pumpProcess, 
        IUserService userService, IHubContext<SmartPumpHub> smartPumpHub)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pumpProcess = pumpProcess;
            _userService = userService;
            _smartPumpHub = smartPumpHub;
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
        public Result<Response> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            try
            {
                _unitOfWork.Begin();
                bool successfull_transaction = _unitOfWork.GenericConfigValuesRepository.UpdatePumpServiceMode(pumpServiceMode);

                if (successfull_transaction)
                {
                    _unitOfWork.Commit();
                    _messageManager.SendMsg("POST", "REQ_APPLY_GRAL_NEW_CONFIG", "");
                    _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");

                    return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("El modo de servicio ha sido cambiado", success: true));
                }
                else
                {
                    _unitOfWork.Rollback();
                    _logger.LogError($"Error in the transaction, check database transaction for more details.");
                    return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("Error cambiando el modo de servicio del lado", "Error in the transaction, check database transaction.", true));
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating pump service mode: {e.Message}");
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error cambiando el modo de servicio del lado.", $"ERROR: {e.Message}"));
            }
        }

        public Result<Response> ExecutePumpAction(PumpAction pumpAction)
        {
            try
            {
                Pump pump = SmartPumpPersistence.GetPump(pumpAction.Pump);
                pump.SetAthoredStatus(pumpAction.Action);
                SmartPumpPersistence.UpdatePump(pump);
                
                _smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);

                _pumpProcess.ExecutePumpAction(pumpAction);
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("Accion ejecutada sobre el lado.", success: true));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error executing pump action: {e.Message}");
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error ejecutando la accion {pumpAction.Action} sobre el lado {pumpAction.Pump}.", $"ERROR executing pump action: {e.Message}"));
            }
        }

        public Result<ResponseWithList<PumpServiceMode>> GetPumpsAndServicesModes()
        {
            try
            {
                IEnumerable<PumpServiceMode> list = _unitOfWork.ConfigValuesRepository.GetPumpsAndServicesModes().ToList();
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponseList<PumpServiceMode>(list, "Modos de servicios obtenidos.", success: true));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting pump services modes: {e.Message}");
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponseList<PumpServiceMode>(null, "Error obteniendo la lista de 'ServicesModes'.", $"ERROR getting pump services modes: {e.Message}"));
            }
        }

        public Result<ResponseWithList<Sale>> GetSales(int pumpNo)
        {
            try
            {
                IEnumerable<Sale> list = SmartSalePersistence.GetSalesByPump(pumpNo);
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponseList<Sale>(list, $"Ventas del lado {pumpNo} obtenidas", success: true));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting pumpNo {pumpNo} sales: {e.Message}");
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponseList<Sale>(null, $"Error obteniendo las ventas del lado {pumpNo}.", $"ERROR: getting pump {pumpNo} sales: {e.Message}"));
            }
        }

        public Result<Response> SendPresent(PresetConfig presetConfig)
        {
            try
            {
                _userService.LogInUser();

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

                    data += $"|GR={grades}";
                }

                data += "|";

                _messageManager.SendMsg("POST", eventType, data);

                if (_userService.UserIsConnected(_messageManager.GetLastReply()))
                    return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse("El 'Preset' fue enviado al lado.", success: true));
                    
                return SendPresent(presetConfig);
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR sending 'Preset' to pump {presetConfig.PumpNo}: {e.Message}");
                return ResponseHelper.NewResult(StatusCode.Ok, ResponseHelper.NewResponse($"Error enviando el 'Preset' al lado {presetConfig.PumpNo}.", $"ERROR sending 'Preset' to pump: {e.Message}"));
            }
        }
    }
}

