using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Communication;

namespace SmartTrinity.App.Services
{

    public class SocketManagerService
    {
        private readonly ICommunicationManager _messageManager;
        private readonly ILogger<SocketManagerService> _logger;

        public SocketManagerService()
        {
            
        }
    }
}
