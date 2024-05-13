using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
