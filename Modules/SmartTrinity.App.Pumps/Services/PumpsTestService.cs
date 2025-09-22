using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Pumps.Persistence;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Pumps.Core.Database;
using SmartTrinity.App.Core.Communication.Adapters.Tcp.Extensions;

namespace SmartTrinity.App.Pumps.Services
{
    public class PumpsTestService : IPumpsTestService
    {
        IPumpsUnitOfWork _pumpsUnitOfWork;
        ICommunicationManager _communicationManager;
        private const string PUMP_STATUS_ERROR = "ERROR";

        public PumpsTestService(IPumpsUnitOfWork pumpsUnitOfWork, ICommunicationManager communicationManager)
        {
            _pumpsUnitOfWork = pumpsUnitOfWork;
            _communicationManager = communicationManager;
        }



        public async Task<string> SendTestPresent(int pumpNo)
        {
            try
            {
                var pump = PumpsPersistence.Get(pumpNo);
                if (pump == null)
                    return $"Error enviando el 'Preset' al lado {pumpNo}. No existe en el listado de pumps";
                else if (pump.Status == PUMP_STATUS_ERROR)
                    return $"Error enviando el 'Preset' al lado {pumpNo}. El pump esta en estado de error.";

                string eventType = $"REQ_PUMP_BOGUS_HANDLE_ID_{pumpNo.ToString().LPad("0", 3)}";

                _communicationManager.SendMsg("POST", eventType, "");

                return "El 'Preset' fue enviado al lado.";
            }
            catch (Exception e)
            {
                //_logger.LogError($"ERROR sending 'Preset' to pump {pumpNo}: {e.Message}");
                return $"Error enviando el 'Preset' al lado {pumpNo}.";
            }
        }

    }
}
