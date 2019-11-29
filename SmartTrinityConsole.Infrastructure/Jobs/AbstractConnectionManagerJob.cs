using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;

namespace SmartTrinityConsole.Infrastructure.Jobs
{
    public abstract class AbstractConnectionManager
    {
        protected IPumpService _pumpService;
        protected ICommunicationManager _messageManager;
        protected IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);

        protected ILogger<AbstractConnectionManager> _logger;

        public AbstractConnectionManager(ILogger<AbstractConnectionManager> logger, ICommunicationManager messageManager, IPumpService pumpService)
        {
            _logger = logger;

            _pumpService = pumpService;
            _messageManager = messageManager;
            _messageManager.ConnectionParams = connectionParams;
        }

        public virtual void StartConnection()
        {
            _messageManager.Connect();
            _pumpService.PumpsBaseConfig();
        }

        public virtual void CheckConnectionStatus()
        {
            bool startJob = false;
            try
            { _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", ""); }
            catch (System.Exception e)
            {
                _logger.LogDebug(e.Message);
                string msg = e.Message.Split('|')[1];

                if (msg == "Need to be connected before subscribing messages")
                {
                    _logger.LogInformation("Connecting to the socket.");
                    startJob = true;
                }
            }

            if (startJob)
            {
                StartConnection();
            }
        }

        public virtual void DisconnectFromServer()
        {

        }
    }
}