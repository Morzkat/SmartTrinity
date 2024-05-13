namespace SmartTrinity.Infrastructure.Database
{

    public class DapperDatabaseManager
    {
        public static void Setup()
        {
            // Dapper.CustomPropertyTypeMap
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            // Mappers
            // FluentMapper.Initialize(config =>
            // {
            //     config.AddMap(new GenericConfigValuesMapper());
            //     config.ForDommel();
            // });
        }
    }
}