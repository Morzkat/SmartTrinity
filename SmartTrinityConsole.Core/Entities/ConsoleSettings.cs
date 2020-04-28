namespace SmartTrinityApi.Core.Entities 
{
    public class ConsoleSettings 
    {
        public Credentials Credentials { get; set; }
    }

    public class Credentials 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}