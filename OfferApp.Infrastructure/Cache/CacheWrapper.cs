using System.Runtime.Caching;

namespace OfferApp.Infrastructure.Cache
{
    // MemoryCache nie dostarcza interfejsu i z tego powodu zastosowałem wrapper.
    // Dodatkowo MemoryCache obudowuje każdy element jako CacheItem.
    // MemoryCache do każdej wartości podochodzi indywidualnie tzn każdemu obiektowi można ustawić własne jego policy.
    // My akurat potrzebujemy globalnych ustawień dlatego przekazałem CacheOptions w konstruktorze
    internal sealed class CacheWrapper : ICacheWrapper
    {
        private readonly MemoryCache _cache;
        private readonly CacheOptions _options;

        public CacheWrapper(CacheOptions cacheOptions)
        {
            _cache = new MemoryCache("OfferApp");
            _options = cacheOptions;
        }

        public T Add<T>(string key, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _cache.Add(new CacheItem(key, item), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(_options.CacheEntryExpired)
            });
            return item;
        }

        public void Delete(string key)
        {
            if (_cache.Get(key) is not null)
            {
                _cache.Remove(key);
            }
        }

        public T? Get<T>(string key)
        {
            return (T?) _cache.Get(key);
        }

        public T Update<T>(string key, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_cache.Get(key) is not null)
            {
                _cache.Remove(key);
            }

            _cache.Add(new CacheItem(key, item), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(_options.CacheEntryExpired)
            });
            return item;
        }
    }
}
