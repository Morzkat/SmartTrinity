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
using SmartTrinityConsole.Core.ServerResponse;
using SmartTrinityApi.Core.ServerResponse.Helpers;
using System;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityConsole.Infrastructure.Persistence;

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

        public PumpService(ILogger<PumpService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IUnitOfWork unitOfWork, IPumpProcess pumpProcess, IUserService userService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pumpProcess = pumpProcess;
            _userService = userService;
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
        public Response UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
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

                    return ResponseHelper.NewResponse("El modo de servicio ha sido cambiado", success: true);
                }
                else
                {
                    _unitOfWork.Rollback();
                    _logger.LogError($"Error in the transaction, check database transaction for more details.");
                    return ResponseHelper.NewResponse("Error cambiando el modo de servicio del lado", "Error in the transaction, check database transaction.", true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating pump service mode: {e.Message}");
                return ResponseHelper.NewResponse($"Error cambiando el modo de servicio del lado.", $"ERROR: {e.Message}");
            }
        }

        public Response ExecutePumpAction(PumpAction pumpAction)
        {
            try
            {
                _pumpProcess.ExecutePumpAction(pumpAction);
                return ResponseHelper.NewResponse("Accion ejecutada sobre el lado.", success: true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error executing pump action: {e.Message}");
                return ResponseHelper.NewResponse($"Error ejecutando la accion {pumpAction.Action} sobre el lado {pumpAction.Pump}.", $"ERROR executing pump action: {e.Message}");
            }
        }

        public ResponseWithList<PumpServiceMode> GetPumpsAndServicesModes()
        {
            try
            {
                IEnumerable<PumpServiceMode> list = _unitOfWork.ConfigValuesRepository.GetPumpsAndServicesModes().ToList();
                return ResponseHelper.NewResponseList<PumpServiceMode>(list, "Modos de servicios obtenidos.", success: true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting pump services modes: {e.Message}");
                return ResponseHelper.NewResponseList<PumpServiceMode>(null, "Error obteniendo la lista de 'ServicesModes'.", $"ERROR getting pump services modes: {e.Message}");
            }
        }

        public ResponseWithList<Sale> GetSales(int pumpNo)
        {
            try
            {
                IEnumerable<Sale> list = SmartSalePersistence.GetSalesByPump(pumpNo);
                return ResponseHelper.NewResponseList<Sale>(list, $"Ventas del lado {pumpNo} obtenidas", success: true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting pumpNo {pumpNo} sales: {e.Message}");
                return ResponseHelper.NewResponseList<Sale>(null, $"Error obteniendo las ventas del lado {pumpNo}.", $"ERROR: getting pump {pumpNo} sales: {e.Message}");
            }
        }

        public Response SendPresent(PresetConfig presetConfig)
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

                return ResponseHelper.NewResponse("El 'Preset' fue enviado al lado.", success: true);
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR sending 'Preset' to pump {presetConfig.PumpNo}: {e.Message}");
                return ResponseHelper.NewResponse($"Error enviando el 'Preset' al lado {presetConfig.PumpNo}.", $"ERROR sending 'Preset' to pump: {e.Message}");
            }
        }
    }
}

