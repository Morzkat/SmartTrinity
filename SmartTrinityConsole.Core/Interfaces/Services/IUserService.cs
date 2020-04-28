using System;
using System.Collections.Generic;
using System.Text;
using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Process
{
    public interface IUserService
    {
        void LogInUser();
        bool UserIsConnected();
    }
}
