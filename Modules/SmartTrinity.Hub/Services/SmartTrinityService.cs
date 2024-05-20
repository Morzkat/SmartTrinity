using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;
using SmartTrinity.App.Infrastructure.Communication.Adapaters.Tcp;

namespace SmartTrinity.App.Services
{
    public class SmartTrinityService : ISmartTrinityService
    {
        private readonly ILogger<ISmartTrinityService> _logger;
        private readonly ICommunicationManager _communicationManager;
        protected IConnectionParams _connectionParams = new TcpConnectionParams("148.0.241.122", 3011);

        public SmartTrinityService(ILogger<ISmartTrinityService> logger, ICommunicationManager communicationManager)
        {
            _logger = logger;
            _communicationManager = communicationManager;
            _communicationManager.ConnectionParams = _connectionParams;
        }

        public void Setup()
        {
            _communicationManager.Connect();
            SetupRequestConfigurations();
            while (Thread.CurrentThread.IsAlive)
            {
                _communicationManager.ReceiveSubscribedMessages();
                //_serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);

                if (_communicationManager.MsgType.Equals("RES_FCRT_PUMPS_CONFIG"))
                    break;
            }
            SetupSubscriptionsToEvents();
        }

        public async Task HandleClientAsync()
        {
            while (true)
            {
                //var handler = await _communicationManager.GetSocketHandler();

                await Task.Run(() =>
                {
                    if (_communicationManager.SocketHasData())
                        _communicationManager.ReceiveSubscribedMessages();

                    if (!_communicationManager.ClientIsConnected())
                        Setup();
                });
            }
        }

        public void SetupRequestConfigurations()
        {
            _communicationManager.SendMsg("POST", "REQ_FCRT_GET_GRAL_CONFIG", "");
            _communicationManager.SendMsg("POST", "REQ_FCRT_GRADES_CONFIG", "");
            _communicationManager.SendMsg("POST", "REQ_LAST_CONFIG_ID", "CT=DC|");
            _communicationManager.SendMsg("POST", "REQ_LAST_CONFIG_ID", "REQ=REQ_FCRT_GET_GRAL_CONFIG|");
            _communicationManager.SendMsg("POST", "REQ_FCRT_PUMPS_CONFIG", $"PC={GetComputerId()}|");
        }

        public void SetupSubscriptionsToEvents()
        {
            _communicationManager.Subscribe("EVT_NEW_CONFIG_APPLIED");
            _communicationManager.Subscribe("EVT_ENTRY_PUMP_CONTROLLER");
            _communicationManager.Subscribe("EVT_EXIT_PUMP_CONTROLLER");
            _communicationManager.Subscribe("EVT_PUMP_ERROR_MSG_ID_*");
            _communicationManager.Subscribe("EVT_PUMP_STATUS_CHANGE_ID_*");
            _communicationManager.Subscribe("EVT_PUMP_NEW_TRANSACTION");
            _communicationManager.Subscribe("EVT_PUMP_PRICE_LEVEL_ID_*");
            _communicationManager.Subscribe("EVT_GRADE_PRICE_CHANGE");
            _communicationManager.Subscribe("EVT_PUMP_DELIVERY_PROGRESS_ID_*");
            _communicationManager.Subscribe("EVT_PUMP_TOTALIZER_UPDATE_ID_*");
        }

        private static string GetComputerId()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }
    }
}
