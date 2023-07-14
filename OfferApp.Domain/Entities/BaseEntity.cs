namespace OfferApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }

        public BaseEntity(int id)
        {
            Id = id;
        }
    }
}
