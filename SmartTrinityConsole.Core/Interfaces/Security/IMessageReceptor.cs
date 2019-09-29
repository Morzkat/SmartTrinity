using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Interfaces.Security 
{
    public interface IMessageReceptor 
    {
        void Receive(ICommunicationManager communicationManager);
    }
}