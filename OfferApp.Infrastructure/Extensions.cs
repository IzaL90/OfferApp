using Microsoft.Extensions.DependencyInjection;
using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using OfferApp.Infrastructure.Cache;
using OfferApp.Infrastructure.Database;
using OfferApp.Infrastructure.Repositories;
using System.Data;
using System.Text.Json;
using OfferApp.Migrations;
using MySql.Data.MySqlClient;

namespace OfferApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            //return services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            return services.AddScoped(typeof(IRepository<>), typeof(XmlRepository<>))
                .AddScoped<BidRepository>()
                .RegisterCache(appsettings)
                .AddScoped<IRepository<Bid>>(sp =>
                {
                    var repository = sp.GetRequiredService<BidRepository>();
                    var cacheWrapper = sp.GetRequiredService<ICacheWrapper>();
                    return new CacheRepository<Bid>(repository, cacheWrapper);
                })
                .AddDatabase(appsettings);
        }

        public static IServiceProvider UseInfrastructure(this IServiceProvider serviceProvider)
        {
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Start();
            return serviceProvider;
        }

        private static IServiceCollection RegisterCache(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            var cacheOptionsJsonElement = (JsonElement)(appsettings["cache"]
                ?? throw new InvalidOperationException("Cannot find section 'cache', please ensure that this file exists"));
            var cacheOptions = cacheOptionsJsonElement.Deserialize<CacheOptions>(new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            }) ?? throw new InvalidOperationException("Cannot deserialize section 'cache', please ensure that this section is correct");
            return services.AddSingleton(cacheOptions)
                           .AddSingleton<ICacheWrapper, CacheWrapper>();
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            var databaseOptionsJsonElement = (JsonElement)(appsettings["database"]
                ?? throw new InvalidOperationException("Cannot find section 'database', please ensure that this file exists"));
            var databaseOptions = databaseOptionsJsonElement.Deserialize<DatabaseOptions>(new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            }) ?? throw new InvalidOperationException("Cannot deserialize section 'database', please ensure that this section is correct");
            if (string.IsNullOrWhiteSpace(databaseOptions.ConnectionString))
            {
                throw new InvalidOperationException("Check in 'database' section 'connectionString' if is correct");
            }
            services.AddSingleton(databaseOptions);
            services.AddTransient<IDbInitializer, DbInitializer>();
            services.AddMigrations(databaseOptions.ConnectionString);

            services.AddScoped<IDbConnection, MySqlConnection>(sp =>
            {
                return new MySqlConnection(databaseOptions.ConnectionString);
            });
            return services;
        }
    }
}