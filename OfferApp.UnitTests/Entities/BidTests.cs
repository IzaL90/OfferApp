using OfferApp.Core.Entities;
using Shouldly;

namespace OfferApp.UnitTests.Entities
{
    public class BidTests
    {
        [Fact]
        public void ShouldCreateBid()
        {
            var name = "name";
            var description = "description-test12345";
            var firstPrice = 100;

            var bid = Bid.Create(name, description, firstPrice);

            bid.ShouldNotBeNull();
            bid.Name.ShouldBe(name);
            bid.Description.ShouldBe(description);
            bid.FirstPrice.ShouldBe(firstPrice);
        }
    }
}