using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;

namespace SmartTrinity.App.Infrastructure.Communication.Adapaters.Tcp
{
    public class TcpConnectionParams : IConnectionParams
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public long ConnectionTimeOut { get; set; }

        public TcpConnectionParams(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public ITcpCommunication BuildClient()
        {
            var communication = new TcpCommunication();
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