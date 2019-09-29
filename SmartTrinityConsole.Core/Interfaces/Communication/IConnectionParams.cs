using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Communication
{
    public interface IConnectionParams
    {
        string portName { get; set; }
        long connectionTimeOut { get; set; }

        ICommunication BuildClient();

        Dictionary<string, string> GetParams();
    }
}
