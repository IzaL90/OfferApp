using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace OfferApp.Migrations
{
    public static class Extensions
    {
        public static IServiceCollection AddMigrations(this IServiceCollection services, string connectionString)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(cr =>
                        cr.AddMySql5()
                          .WithGlobalConnectionString(connectionString)
                          .ScanIn(typeof(CreateTableBid).Assembly).For.Migrations())
                .AddLogging(l => l.AddFluentMigratorConsole());
            return services;
        }
    }
}