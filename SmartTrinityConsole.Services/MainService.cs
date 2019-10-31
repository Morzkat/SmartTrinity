using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Tcp.Communication;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class MainService : IMainService
    {
        IPumpService _pumpService;
        private IMemoryCache _cache;
        ILogger<MainService> _logger;
        IServiceProcess _serviceProcess;
        ICommunicationManager _messageManager;
        // Test params
        IConnectionParams connectionParams = new TcpConnectionParams("127.0.0.1", 3011);

        public MainService(ILogger<MainService> logger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
            _pumpService = pumpService;
            _messageManager = messageManager;
            _serviceProcess = serviceProcess;
            _messageManager.ConnectionParams = connectionParams;

            _messageManager.Connect();
            _pumpService.PumpsBaseConfig();

        }

        public void ReadFromSocketContinuously()
        {
            DateTime cacheEntry = DateTime.Now;

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(6));
            _cache.Set("LastCheckedTime", cacheEntry, cacheEntryOptions);
            
            Task pumpSellingProcessTask = new Task(() =>
            {
                while (true)
                {
                    _cache.TryGetValue("LastCheckedTime", out cacheEntry);

                    if (_messageManager.SocketHasData())
                    {
                        _messageManager.ReceiveSubscribedMessages();
                        var result = _serviceProcess.ProcessMessage(_messageManager.MsgType, _messageManager.MsgData);
                    }
                    else if (DateTime.Now.Minute - cacheEntry.Minute > 3)
                    {
                        _messageManager.SendMsg("POST", "REQ_REFRESH_GENERAL_INFORMATION", "");
                         _cache.Set("LastCheckedTime", DateTime.Now, cacheEntryOptions);
                        _logger.LogDebug("Sending Message: REQ_REFRESH_GENERAL_INFORMATION...");
                    }
                }
            });
            pumpSellingProcessTask.Start();
        }
    }
}
