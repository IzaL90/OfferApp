using System.Runtime.Serialization;

namespace OfferApp.Core.Entities
{
    [DataContract] // atrybut niezbędny do serializaci i deserializacji prywatnych właściwości
    public abstract class BaseEntity
    {
        [DataMember] // atrybut niezbędny do serializaci i deserializacji prywatnych właściwości
        // wystąpił problem z readonly właściwością i z tego powodu aby nie komplikować ustawiłem private set
        public int Id { get; private set; }

        public BaseEntity(int id)
        {
            Id = id;
        }
    }
}
