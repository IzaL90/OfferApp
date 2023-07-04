using Microsoft.Extensions.DependencyInjection;

namespace OfferApp.Infrastructure.Cache
{
    internal static class Extensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICacheWrapper>(sp =>
            {
                var cacheOptions = sp.GetRequiredService<CacheOptions>();
                return new CacheWrapper(cacheOptions);
            });
        }
    }
}
