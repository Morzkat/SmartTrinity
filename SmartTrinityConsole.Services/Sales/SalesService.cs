using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Interfaces.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityConsole.Services.Sales
{
    public class SalesService: ISalesServices
    {
        ILogger<SalesService> _logger;
        ICommunicationManager _messageManager;
        public SalesService(ILogger<SalesService> logger, ICommunicationManager messageManager) 
        {
            _logger = logger;
            _messageManager = messageManager;
        }

    }
}
