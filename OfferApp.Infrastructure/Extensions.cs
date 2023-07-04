using Microsoft.Extensions.DependencyInjection;
using OfferApp.Infrastructure.Cache;
using OfferApp.Infrastructure.Database;
using System.Text.Json;

namespace OfferApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            return services.RegisterAppSettings(appsettings)
                           .AddDatabase();
        }

        private static IServiceCollection RegisterAppSettings(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            return services.RegisterCacheOptions(appsettings)
                           .RegisterDatabaseOptions(appsettings);
        }

        public static IServiceProvider UseInfrastructure(this IServiceProvider serviceProvider)
        {
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Start();
            return serviceProvider;
        }

        private static IServiceCollection RegisterCacheOptions(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            var cacheOptionsJsonElement = (JsonElement)(appsettings["cache"]
                ?? throw new InvalidOperationException("Cannot find section 'cache', please ensure that this file exists"));
            var cacheOptions = cacheOptionsJsonElement.Deserialize<CacheOptions>(new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            }) ?? throw new InvalidOperationException("Cannot deserialize section 'cache', please ensure that this section is correct");
            return services.AddSingleton(cacheOptions);
        }

        private static IServiceCollection RegisterDatabaseOptions(this IServiceCollection services, Dictionary<string, object> appsettings)
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
            return services;
        }

        // metoda wprowadzona aby umożliwić dostęp do serwisów podczas rejestrowania w kontenerze IoC
        // podczas rejestracji sporej ilości serwisów może powodować opóźnienie w odpaleniu aplikacji
        public static T GetService<T>(this IServiceCollection services)
             where T : notnull
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }
    }
}