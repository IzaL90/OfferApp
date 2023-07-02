using Microsoft.Extensions.DependencyInjection;
using OfferApp.Core.Repositories;
using OfferApp.Infrastructure.Repositories;

namespace OfferApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //return services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            // podmiana na repozytorium w pliku xml
            return services.AddScoped(typeof(IRepository<>), typeof(XmlRepository<>));
        }
    }
}