namespace OfferApp.Shared.DTO
{
    public class BidPublishedDto : IBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int Count { get; set; }
        public decimal FirstPrice { get; set; }
        public decimal? LastPrice { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Created: {Created}, FirstPrice: {FirstPrice}, "
                + $"Updated: {Updated}, Count: {Count}, " +
                $"LastPrice {LastPrice}, Description: {Description}";
        }
    }
}
