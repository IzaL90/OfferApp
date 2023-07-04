using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfferApp.Infrastructure.Repositories;
using OfferApp.Migrations;

namespace OfferApp.Infrastructure.Database
{
    internal static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddEFCore();
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var databaseOptions = services.GetService<DatabaseOptions>();
            var serverVersion = ServerVersion.AutoDetect(databaseOptions.ConnectionString);
            services.AddDbContext<OfferDbContext>(options => options.UseMySql(databaseOptions.ConnectionString, serverVersion));
            return services;
        }

        internal static IServiceCollection AddFileStoreProvider(this IServiceCollection services)
        {
            services.AddFileRepositories();
            return services;
        }

        internal static IServiceCollection AddDapper(this IServiceCollection services)
        {
            services.AddDapperRepositories();
            services.AddTransient<IDbInitializer, DapperDbInitializer>();
            var databaseOptions = services.GetService<DatabaseOptions>();
            services.AddMigrations(databaseOptions.ConnectionString!);
            return services;
        }

        internal static IServiceCollection AddEFCore(this IServiceCollection services)
        {
            services.AddDbContext();
            services.AddEFCoreRepositories();
            services.AddTransient<IDbInitializer, EFDbInitializer>();
            return services;
        }
    }
}
