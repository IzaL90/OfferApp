using OfferApp.Core.Entities;

namespace OfferApp.IntegrationTests.Common
{
    internal static class Fixtures
    {
        public static Bid CreateBid()
        {
            return Bid.Create("Name#1", "Description12345", 100);
        }

        public static Bid CreatePublishedBid()
        {
            var bid = CreateBid();
            bid.Publish();
            return bid;
        }
    }
}
