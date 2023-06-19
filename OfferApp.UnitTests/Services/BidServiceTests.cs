using Moq;
using OfferApp.Core.Entities;
using OfferApp.Core.Exceptions;
using OfferApp.Core.Mappings;
using OfferApp.Core.Repositories;
using OfferApp.Core.Services;
using Shouldly;

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

        [Fact]
        public void ShouldDeleteBid()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).Returns(bid);

            _service.DeleteBid(bid.Id);

            _bidRepository.Verify(b => b.Delete(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public void GivenNotExistingBid_WhenDeleteBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();
            
            var exception = Record.Exception(() => _service.DeleteBid(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public void ShouldUpdateBid()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).Returns(bid);

            _service.UpdateBid(bid.AsDto());

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public void GivenNotExistingBid_WhenUpdateBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();
            
            var exception = Record.Exception(() => _service.UpdateBid(bid.AsDto()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public void GivenNotExistingBid_WhenPublishedBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = Record.Exception(() => _service.Published(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public void ShouldChangeBidToPublished()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).Returns(bid);

            _service.Published(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public void GivenNotExistingBid_WhenUnpublishedBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = Record.Exception(() => _service.Unpublished(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public void ShouldChangeBidToUnpublished()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).Returns(bid);

            _service.Unpublished(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public void GivenNotExistingBid_WhenBidUp_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = Record.Exception(() => _service.BidUp(bid.Id, 100000));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public void ShouldBidUp()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).Returns(bid);

            _service.BidUp(bid.Id, 100000);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
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
