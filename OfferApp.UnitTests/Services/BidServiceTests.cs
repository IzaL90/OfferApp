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
        public async Task ShouldAddBid()
        {
            var bid = Common.CreateBid();

            await _service.AddBid(bid.AsDto());

            _bidRepository.Verify(b => b.Add(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public async Task ShouldDeleteBid()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.DeleteBid(bid.Id);

            _bidRepository.Verify(b => b.Delete(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenDeleteBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();
            
            var exception = await Record.ExceptionAsync(() => _service.DeleteBid(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task ShouldUpdateBid()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.UpdateBid(bid.AsDto());

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenUpdateBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();
            
            var exception = await Record.ExceptionAsync(() => _service.UpdateBid(bid.AsDto()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenPublishedBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = await Record.ExceptionAsync(() => _service.Published(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task ShouldChangeBidToPublished()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.Published(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public async Task GivenBidPublished_WhenPublishedBid_ShouldntUpdateBid()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.Published(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Never);
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenUnpublishedBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = await Record.ExceptionAsync(() => _service.Unpublished(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task ShouldChangeBidToUnpublished()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.Unpublished(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        [Fact]
        public async Task GivenBidUnpublished_WhenUnpublishedBid_ShouldntUpdateBid()
        {
            var bid = Common.CreateBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.Unpublished(bid.Id);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Never);
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenBidUp_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = await Record.ExceptionAsync(() => _service.BidUp(bid.Id, 100000));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task ShouldBidUp()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            await _service.BidUp(bid.Id, 100000);

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
