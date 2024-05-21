using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;
using SmartTrinity.App.Infrastructure.Communication.Adapaters.Tcp;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Core.Models;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp.Extensions;

namespace SmartTrinity.App.Services
{
    public class SmartTrinityService : ISmartTrinityService
    {
        private readonly ConsoleSettings _consoleSettings;
        private readonly ILogger<ISmartTrinityService> _logger;
        private readonly ICommunicationManager _communicationManager;
        private readonly IMessageService _messageService;
        protected IConnectionParams _connectionParams;

        public SmartTrinityService(ILogger<ISmartTrinityService> logger, ICommunicationManager communicationManager, IOptions<ConsoleSettings> consoleSettingsOptions,
            IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
            _consoleSettings = consoleSettingsOptions.Value;
            _connectionParams = new TcpConnectionParams(_consoleSettings.Host, _consoleSettings.Port);
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
                _messageService.Process(_communicationManager.MsgType, _communicationManager.MsgData);

                if (_communicationManager.MsgType.Equals("RES_FCRT_PUMPS_CONFIG"))
                    break;
            }
            SetupSubscriptionsToEvents();
            LoginToService();
        }

        public async Task HandleClientAsync()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    if (_communicationManager.SocketHasData())
                    {
                        _communicationManager.ReceiveSubscribedMessages();
                        _messageService.Process(_communicationManager.MsgType, _communicationManager.MsgData);
                    }

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

        private void LoginToService()
        {
            string credentials = GetUserCredentials();
            _communicationManager.SendMsg("POST", "REQ_SECU_LOGIN", credentials);
        }

        private string GetUserCredentials()
        {
            string user = _consoleSettings.Credentials.Username.RPad(" ", 25);
            string pw = _consoleSettings.Credentials.Username.RPad(" ", 25);

            char[] encryptedPw = pw.ToCharArray().EncryptMessage(25, user.ToCharArray(), 20);
            string data = $"US=1|PW={encryptedPw.ConvertBinToHex(25)}|";

            return data;
        }

        private static string GetComputerId()
        {
            return System.Net.Dns.GetHostName().ToUpper();
        }
    }
}
