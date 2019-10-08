using System.Collections.Generic;
using SmartTrinityApi.Core.Interfaces;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Services.Tcp.Communication
{
    public class TcpConnectionParams : IConnectionParams
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public long ConnectionTimeOut { get; set; }

        public TcpConnectionParams()
        {

        }

        public TcpConnectionParams(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public ICommunication BuildClient()
        {
            ICommunication communication = new TcpCommunication();
            communication.SetParams(this);
            return communication;
        }

        public Dictionary<string, string> GetParams()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"TCPIP: {Host} ip: {Port}";
        }
    }
}