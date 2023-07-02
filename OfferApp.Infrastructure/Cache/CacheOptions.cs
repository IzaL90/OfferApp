namespace OfferApp.Infrastructure.Cache
{
    // opcje cache
    internal class CacheOptions
    {
        // czas po którym obiekt zostanie usunięty
        public TimeSpan CacheEntryExpired { get; set; }
    }
}
