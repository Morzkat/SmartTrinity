namespace SmartTrinity.App.Migrations.Core.Services
{
    public interface ISalesMigrator
    {
        Task MigrateSales();
        Task MigrateSalesToCentral();
    }
}
