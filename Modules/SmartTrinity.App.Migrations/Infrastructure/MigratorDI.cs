using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace SmartTrinity.App.Migrations.Infrastructure
{
    public static class MigratorDI
    {
        public static IServiceCollection SetupMigrators([NotNull] this IServiceCollection serviceCollection)
        {
            //TODO: Find a way to inject the repositories and share the same context without creating a instance.
            return serviceCollection;
        }
    }
}
