using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfferApp.Infrastructure.Repositories;

namespace OfferApp.Infrastructure.Database
{
    internal static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var databaseOptions = services.GetService<DatabaseOptions>();
            var serverVersion = ServerVersion.AutoDetect(databaseOptions.ConnectionString);
            services.AddDbContext<OfferDbContext>(options => options.UseMySql(databaseOptions.ConnectionString, serverVersion));
            services.AddEFCoreRepositories();
            services.AddTransient<IDbInitializer, EFDbInitializer>();
            return services;
        }
    }
}
