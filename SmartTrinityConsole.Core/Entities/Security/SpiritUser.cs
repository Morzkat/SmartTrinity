using SmartTrinityConsole.Interfaces.Security;

namespace SmartTrinityConsole.Entities.Security
{
    public class SpiritUser : IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}