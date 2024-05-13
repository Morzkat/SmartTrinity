using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Communication;
using System.Reflection;

namespace SmartTrinity.App.Services
{
    public interface ICommandService
    {

    }
    public class CommandService: ICommandService
    {
        private readonly ICommunicationManager _communicationManager;
        private readonly ILogger<ICommandService> _logger;

        public CommandService(ILogger<ICommandService> logger, ICommunicationManager communicationManager)
        {
            _logger = logger;    
            _communicationManager = communicationManager;
        }

        public void Execute(string msgType, string msgData)
        {
            int pumpId;
            int.TryParse(msgType.Substring(msgType.Length - 3, 3), out pumpId);

            try
            {
                MethodInfo? _method;
                object? result;
                string? methodName = "";
                if (pumpId != 0)
                {
                    methodName = $"ProcessMessage_{msgType.Substring(0, msgType.Length - 4)}";
                    _method = GetType().GetMethod($"{methodName}");
                    result = _method?.Invoke(this, new object[] { pumpId, msgData });
                }
                else
                {
                    methodName = $"ProcessMessage_{msgType}";
                    _method = GetType().GetMethod($"ProcessMessage_{msgType}");
                    result = _method?.Invoke(this, new object[] { msgData });
                }

            }
            catch { _logger.LogInformation($"Process not found {msgType}"); }
        }


    }
}
