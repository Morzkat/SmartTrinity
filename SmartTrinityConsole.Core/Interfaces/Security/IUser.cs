namespace SmartTrinityConsole.Interfaces.Security 
{
    public interface IUser 
    {
        string UserName { get; set; }

        string Password { get; set; } 
    }
}