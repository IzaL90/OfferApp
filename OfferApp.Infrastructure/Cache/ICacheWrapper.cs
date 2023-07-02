using System.Runtime.Caching;

namespace OfferApp.Infrastructure.Cache
{
    internal interface ICacheWrapper
    {
        T Add<T>(string key, T item);

        T? Get<T>(string key);

        T Update<T>(string key, T item);

        void Delete(string key);
    }
}
