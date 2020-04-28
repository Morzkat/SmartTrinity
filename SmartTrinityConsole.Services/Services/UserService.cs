
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class UserService : IUserService
    {

        ICommunicationManager _messageManager;
        ILogger<UserService> _logger;

        public UserService(ICommunicationManager messageManager, ILogger<UserService> logger)
        {
            _messageManager = messageManager;
            _logger = logger;
        }

        public void LogInUser()
        {
            if (!UserIsConnected() || !SmartUserPersistence.UserIsLogged)
            {
                string dataForLogin = SmartUserPersistence.PrepareDataForLogin();
                _messageManager.SendMsg("POST", "REQ_SECU_LOGIN", dataForLogin);
                SmartUserPersistence.UserIsLogged = true;
            }
        }

        // HACK: Look for way to refactor this code...
        public bool UserIsConnected()
        {
            _logger.LogDebug(SmartUserPersistence.LastReply);
            var data = SmartUserPersistence.LastReply.Split("|");

            if (data.Length == 0 || data[0] == "")
                return false;

            else if (data[1] == "RES_SECU_LOGIN")
            {
                var response = data[4].Split("=")[1];
                if (response == "DENIED")
                {
                    SmartUserPersistence.UserIsLogged = false;
                    return false;
                }
            }
            else if (data[1] == "RES_SECU_ACCESS_DENIED")
            {
                var response = data[6].Split("=");
                if (response.Length > 1)
                {
                    if (response[1] == "DENIED")
                    {
                        SmartUserPersistence.UserIsLogged = false;
                        return false;
                    }
                }
                else
                {
                    response = data[4].Split("=");
                    if (response.Length > 1)
                    {
                        if (response[1] == "DENIED")
                        {
                            SmartUserPersistence.UserIsLogged = false;
                            return false;
                        }
                    }
                }
            }
            SmartUserPersistence.LastReply = "User connected to console status | CONNECTED";
            return true;
        }
    }
}
















