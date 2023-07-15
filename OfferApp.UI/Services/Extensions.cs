namespace OfferApp.UI.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddSingleton<IBidService, BidService>();
        }
    }
}
