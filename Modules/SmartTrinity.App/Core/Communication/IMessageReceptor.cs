namespace SmartTrinity.App.Core.Communication
{
    public interface IMessageReceptor
    {
        void Receive(ICommunicationManager communicationManager);
    }
}