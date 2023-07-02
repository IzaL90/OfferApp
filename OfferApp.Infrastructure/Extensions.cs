using Microsoft.Extensions.DependencyInjection;
using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using OfferApp.Infrastructure.Cache;
using OfferApp.Infrastructure.Repositories;

namespace OfferApp.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //return services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            // podmiana na repozytorium w pliku xml
            services.AddScoped(typeof(IRepository<>), typeof(XmlRepository<>))
                // rejestracja jako scoped będzie przydatna w przypadku udekorowania repozytorium używającego cache
                .AddScoped(typeof(XmlRepository<>))
                .AddSingleton<ICacheWrapper, CacheWrapper>(_ => new CacheWrapper(new CacheOptions
                {
                    CacheEntryExpired = TimeSpan.FromMinutes(5),
                }));

            // rejestracja repozytorium które posiada cache
            // można usprawnić to rozwiązanie bazując np na atrybutach i poprzez refleksję odpowiednio rejestrować
            // jednak w tym przypadku wystraczy taka prosta rejesteracja
            services.AddScoped<IRepository<Bid>>(sp =>
            {
                var repository = sp.GetRequiredService<XmlRepository<Bid>>();
                var cacheWrapper = sp.GetRequiredService<ICacheWrapper>();
                return new CacheRepository<Bid>(repository, cacheWrapper);
            });
            return services;
        }
    }
}