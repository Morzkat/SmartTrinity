using SmartTrinity.App.Core.Communication.Adapaters.Tcp;

namespace SmartTrinity.App.Core.Communication.Adapters.Tcp
{
    public interface IConnectionParams
    {
        string Host { get; set; }
        int Port { get; set; }
        long ConnectionTimeOut { get; set; }

        ITcpCommunication BuildClient();

        Dictionary<string, string> GetParams();
    }
}