namespace SmartTrinityConsole.Interfaces.Security 
{
    public interface IUser 
    {
        string GetIdentification { get; set; }

        string GetUserNumber { get; set; } 
    }
}