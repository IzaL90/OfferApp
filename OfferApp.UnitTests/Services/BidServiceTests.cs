using Moq;
using OfferApp.Core.Entities;
using OfferApp.Core.Mappings;
using OfferApp.Core.Repositories;
using OfferApp.Core.Services;

namespace OfferApp.UnitTests.Services
{
    public class BidServiceTests
    {
        [Fact]
        public void ShouldAddBid()
        {
            var bid = Common.CreateBid();

            _service.AddBid(bid.AsDto());

            _bidRepository.Verify(b => b.Add(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        private readonly IBidService _service;
        private readonly Mock<IRepository<Bid>> _bidRepository;

        public BidServiceTests()
        {
            _bidRepository = new Mock<IRepository<Bid>>();
            _service = new BidService(_bidRepository.Object);
        }
    }
}
