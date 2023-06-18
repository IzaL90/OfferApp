using OfferApp.Core.Entities;

namespace OfferApp.Core.Repositories
{
    internal interface IRepository<T>
        where T : BaseEntity
    {
        int Add(T entity);
        bool Update(T entity);
        void Delete(T entity);
        T? Get(int id);
        IReadOnlyList<T> GetAll();
    }
}
