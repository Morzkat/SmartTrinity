using System.Collections.Generic;
using SmartTrinityApi.Core.Interfaces;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Services.Tcp.Communication
{
    public class TcpConnectionParams : IConnectionParams
    {
        public string portName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public long connectionTimeOut { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ICommunication BuildClient()
        {
            return new TcpCommunication();
        }

        public Dictionary<string, string> GetParams()
        {
            throw new System.NotImplementedException();
        }
    }
}