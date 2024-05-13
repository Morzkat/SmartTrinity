namespace SmartTrinity.App.Core.Communication.Adapaters.Tcp
{
    public interface ITcpCommunication : ICommunication
    {
        int GetLocalIdentifier();

        Stream GetInputStream();

        Stream GetOutputStream();

        bool SocketHasData();
        
        int GetLocalPort();
    }
}