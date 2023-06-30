using OfferApp.Core.Entities;

namespace OfferApp.Core.Repositories
{
    internal sealed class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly Dictionary<string, List<T>> _entities = new();

        public Task<int> Add(T entity)
        {
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                entity.Id = 1;
                list = new List<T>() { entity };
                _entities.Add(type.Name, list);
                return Task.FromResult(entity.Id);
            }

            entity.Id = list!.Max(x => x.Id) + 1; 
            list!.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task Delete(T entity)
        {
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            list?.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<T?> Get(int id)
        {
            await Task.CompletedTask;
            var type = typeof(T);
            var containsList = _entities.TryGetValue(type.Name, out var list);

            if (!containsList)
            {
                return null;
            }

            return list?.FirstOrDefault(i => i.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            await Task.CompletedTask;
            var type = typeof(T);
            _entities.TryGetValue(type.Name, out var list);
            return list ?? new List<T>();
        }

        public Task<bool> Update(T entity)
        {
            var type = typeof(T);
            if (_entities.TryGetValue(type.Name, out var list))
            {
                return Task.FromResult(false);
            }

            var index = list!.FindIndex(e => e.Id == entity.Id);

            if (index == -1)
            {
                return Task.FromResult(false);
            }

            list[index] = entity;
            return Task.FromResult(true);
        }
    }
}
