using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Communication
{
    public interface IConnectionParams
    {
        string Host { get; set; }
        int Port { get; set; }
        long ConnectionTimeOut { get; set; }

        ICommunication BuildClient();

        Dictionary<string, string> GetParams();
    }
}
