using System.Text;
using SmartTrinity.App.Pumps.Core.Dtos;
using SmartTrinity.App.Pumps.Persistence;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Pumps.Core.Database;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp.Extensions;

namespace SmartTrinity.App.Pumps.Services
{
    public class PumpsService : IPumpsService
    {
        IPumpsUnitOfWork _pumpsUnitOfWork;
        ICommunicationManager _communicationManager;
        private const string PUMP_STATUS_ERROR = "ERROR";

        public PumpsService(IPumpsUnitOfWork pumpsUnitOfWork, ICommunicationManager communicationManager)
        {
            _pumpsUnitOfWork = pumpsUnitOfWork;
            _communicationManager = communicationManager;
        }

        public IEnumerable<PumpDto> GetPumps()
        {
            var pumpsDto = new List<PumpDto>();
            var pumps = PumpsPersistence.Get();

            foreach (var pump in pumps)
            {
                pumpsDto.Add(new PumpDto
                {
                    Grade = GetGradeDto(pump.Grade),
                    PumpNo = pump.PumpNo,
                    Status = pump.Status,
                    Volume = pump.Volume,
                    SalePrice = pump.SalePrice,
                    PriceLevel = pump.PriceLevel,
                    SaleProgress = pump.SaleProgress,
                    Hoses = GetHoseDtos(pump.Hoses)
                });
            }

            return pumpsDto;
        }

        public async Task<string> ExecuteAction(PumpAction pumpAction)
        {
            try
            {
                SetPumpStatus(pumpAction.PumpNo, pumpAction.Action);

                var action = PumpsPersistence.GetAction(pumpAction.PumpNo);
                if (action == PumpActions.MONEY_PRESET || action == PumpActions.VOLUME_PRESET)
                    _communicationManager.SendMsg("POST", $"REQ_PUMP_CLEAR_PRESET_ID_0{pumpAction.PumpNo.ToString().PadLeft(2, '0')}", "");
                else
                    _communicationManager.SendMsg("POST", $"REQ_PUMP_{pumpAction.Action.ToString().ToUpper()}_ID_0{pumpAction.PumpNo.ToString().PadLeft(2, '0')}", "");

                return "Accion ejecutada sobre el lado.";
                //_smartPumpHub.Clients.All.SendAsync("PumpStatusChange", pump);
            }
            catch (Exception e)
            {
                //_logger.LogError($"Error executing pump action: {e.Message}");
                return $"Error ejecutando la accion {pumpAction.Action} sobre el lado {pumpAction.PumpNo}.";
            }
        }

        public async Task<string> SendPresent(PresetDto preset)
        {
            try
            {
                var pump = PumpsPersistence.Get(preset.PumpNo);
                if (pump == null)
                    return $"Error enviando el 'Preset' al lado {preset.PumpNo}. No existe en el listado de pumps";
                else if (pump.Status == PUMP_STATUS_ERROR)
                    return $"Error enviando el 'Preset' al lado {preset.PumpNo}. El pump esta en estado de error.";

                string eventType = $"REQ_PUMP_PRESET_ID_{preset.PumpNo.ToString().LPad("0", 3)}";
                string data = "TY=" + (preset.Type == 1 ? "MONEY" : "VOLUME");

                if (preset.TankFull)
                    data += $"|VA=FULL";
                else
                    data += $"|VA={preset.Amount}";

                if (preset.Grades != null)
                {
                    var grades = new StringBuilder();
                    foreach (var grade in preset.Grades)
                    {
                        if(!pump.Hoses.Any(h => h.Grades.Any(g => g.Id == grade)))
                            return $"Error enviando el 'Preset' al lado {preset.PumpNo}. El grade no esta asociado al lado.";

                        grades.Append($"{grade},");
                    }
                    grades.Length--;
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

        public void SetupPump(int pumpId)
        {
            _communicationManager.SendMsg("POST", $"REQ_PUMP_STATUS_ID_{pumpId.ToString().LPad("0", 3)}", "");
            _communicationManager.SendMsg("POST", $"REQ_PUMP_CAPABILITIES_ID_{pumpId.ToString().LPad("0", 3)}", "");
            _communicationManager.SendMsg("POST", $"REQ_PUMP_GET_INFO_ID_{pumpId.ToString().LPad("0", 3)}", "");
            _communicationManager.SendMsg("POST", $"REQ_PUMP_GET_ERROR_MSG_ID_{pumpId.ToString().LPad("0", 3)}", "");
            _communicationManager.SendMsg("POST", $"REQ_PUMP_STATUS_ID_{pumpId.ToString().LPad("0", 3)}", "");
            _communicationManager.SendMsg("POST", "REQ_GET_PUMP_SALES", $"PM={pumpId}|QT=12|");
        }

        private void SetPumpStatus(int pumpId, PumpActions action)
        {
            Pump pump = PumpsPersistence.Get(pumpId);
            pump.SetAthoredStatus(action);
            PumpsPersistence.Update(pump);
        }

        private IEnumerable<GradePriceDto> GetPriceDtos(IEnumerable<GradePrice> prices)
        {
            var priceDtos = new List<GradePriceDto>();
            foreach (var price in prices)
            {
                priceDtos.Add(new GradePriceDto
                {
                    Price = price.Price,
                    PriceLevel = price.PriceLevel
                });
            }

            return priceDtos;
        }

        private IEnumerable<HoseDto> GetHoseDtos(IEnumerable<Hose> hoses)
        {
            var hosesDtos = new List<HoseDto>();

            foreach (var hose in hoses)
            {
                hosesDtos.Add(new HoseDto
                {
                    HoseId = hose.HoseId,
                    Grades = GetGradeDtos(hose.Grades),
                    TotalizerMoney = hose.TotalizerMoney,
                    TotalizerVolume = hose.TotalizerVolume
                });
            }

            return hosesDtos;
        }

        private GradeDto GetGradeDto(Grade grade)
        {
            if (grade == null)
                return null;

            return new GradeDto
            {
                Id = grade.Id,
                RGB = grade.RGB,
                Red = grade.Red,
                Blue = grade.Blue,
                Green = grade.Green,
                Description = grade.Description,
            };
        }

        private IEnumerable<GradeDto> GetGradeDtos(IEnumerable<Grade> grades)
        {
            var gradesDto = new List<GradeDto>();

            foreach (var grade in grades)
            {
                gradesDto.Add(GetGradeDto(grade));
            }

            return gradesDto;
        }
    }
}
