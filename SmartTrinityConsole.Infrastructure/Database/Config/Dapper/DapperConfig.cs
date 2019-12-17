namespace  SmartTrinityConsole.Infrastructure.Database.Config 
{
    public static class DapperConfigurations 
    {
        public static void ConfigureDapper () 
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}