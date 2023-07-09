using OfferApp.Core.Entities;
using OfferApp.Core.Repositories;
using OfferApp.Infrastructure.Cache;

namespace OfferApp.Infrastructure.Repositories
{
    internal class CacheRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly IRepository<T> _innerRepository;
        private readonly ICacheWrapper _cacheWrapper;
        private readonly Type type = typeof(T);

        public CacheRepository(IRepository<T> innerRepository, ICacheWrapper cacheWrapper)
        {
            _innerRepository = innerRepository;
            _cacheWrapper = cacheWrapper;
        }


        public async Task<int> Add(T entity)
        {
            var id = await _innerRepository.Add(entity);
            _cacheWrapper.Add(GetCacheKey(entity.Id), entity);
            return id;
        }

        public async Task Delete(T entity)
        {
            await _innerRepository.Delete(entity);
            _cacheWrapper.Delete(GetCacheKey(entity.Id));
        }

        public async Task<T?> Get(int id)
        {
            var entity = _cacheWrapper.Get<T>(GetCacheKey(id));
            if (entity is not null) 
            {
                return entity;
            }

            entity = await _innerRepository.Get(id);
            if (entity is not null)
            {
                _cacheWrapper.Add(GetCacheKey(id), entity);
            }

            return entity;
        }

        public Task<IReadOnlyList<T>> GetAll()
        {
            return _innerRepository.GetAll();
        }

        public async Task<bool> Update(T entity)
        {
            var success = await _innerRepository.Update(entity);
            if (!success)
            {
                return false;
            }

            _cacheWrapper.Update(GetCacheKey(entity.Id), entity);
            return true;
        }

        private string GetCacheKey(int id)
        {
            return $"{type.FullName}#{id}";
        }
    }
}
