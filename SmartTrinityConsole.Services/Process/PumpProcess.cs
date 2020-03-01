using Microsoft.Extensions.Logging;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityApi.Services.Process
{
    public class PumpProcess : IPumpProcess
    {
        IUserService _userService;
        ILogger<PumpProcess> _logger;
        ICommunicationManager _messageManager;

        public PumpProcess(ILogger<PumpProcess> logger, ICommunicationManager messageManager, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _messageManager = messageManager;
        }

        public void AddPump(int pumpId)
        {
            _messageManager.SendMsg("POST", $"REQ_PUMP_CAPABILITIES_ID_{Tools.LPad(pumpId.ToString(), "0", 3)}", "");
            _messageManager.SendMsg("POST", $"REQ_PUMP_GET_INFO_ID_{Tools.LPad(pumpId.ToString(), "0", 3)}", "");
            _messageManager.SendMsg("POST", $"REQ_PUMP_GET_ERROR_MSG_ID_{Tools.LPad(pumpId.ToString(), "0", 3)}", "");
            _messageManager.SendMsg("POST", $"REQ_PUMP_STATUS_ID_{Tools.LPad(pumpId.ToString(), "0", 3)}", "");
            _messageManager.SendMsg("POST", "REQ_GET_PUMP_SALES", $"PM={pumpId}|QT=12|");
        }

        public void CreatePump(int pumpId)
        {
            _messageManager.SendMsg("POST", $"REQ_PUMP_STATUS_ID_{Tools.LPad(pumpId.ToString(), "0", 3)}", "");
        }

        public void DestroyPump(int pumpId)
        {

        }

        // TODO: Use service for call this logic.
        public void ExecutePumpAction(PumpAction pumpAction)
        {
            _userService.LogInUser();
            this._messageManager.SendMsg("POST", $"REQ_PUMP_{pumpAction.Action.ToUpper()}_ID_0{pumpAction.Pump.ToString().PadLeft(2, '0')}", "");

            if (!_userService.UserIsConnected(_messageManager.GetLastReply()))
                ExecutePumpAction(pumpAction);
        }
    }
}
