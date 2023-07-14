using Moq;
using OfferApp.Core.DTO;
using OfferApp.Domain.Entities;
using OfferApp.Domain.Exceptions;
using OfferApp.Core.Exceptions;
using OfferApp.Core.Mappings;
using OfferApp.Domain.Repositories;
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
            exception.ShouldBeOfType<ResourceNotFoundException>();
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
        public async Task PublishedBid_UpdateShouldntBePossible()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);

            var exception = await Record.ExceptionAsync(() => _service.UpdateBid(bid.AsDto()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain($"Bid with id '{bid.Id}' is published and cannot be updated");
            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Never);
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenUpdateBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();
            
            var exception = await Record.ExceptionAsync(() => _service.UpdateBid(bid.AsDto()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task GivenNotExistingBid_WhenPublishedBid_ShouldThrowAnException()
        {
            var bid = Common.CreateBid();

            var exception = await Record.ExceptionAsync(() => _service.Published(bid.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
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
            exception.ShouldBeOfType<ResourceNotFoundException>();
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
            var dto = new BidUpDto { Id = bid.Id, Price = 100000 };

            var exception = await Record.ExceptionAsync(() => _service.BidUp(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task ShouldBidUp()
        {
            var bid = Common.CreatePublishedBid();
            _bidRepository.Setup(b => b.Get(bid.Id)).ReturnsAsync(bid);
            var dto = new BidUpDto { Id = bid.Id, Price = 100000 };

            await _service.BidUp(dto);

            _bidRepository.Verify(b => b.Update(It.Is<Bid>(bi => bi.Id == bid.Id)), times: Times.Once);
        }

        private readonly IBidService _service;
        private readonly Mock<IBidRepository> _bidRepository;

        public BidServiceTests()
        {
            _bidRepository = new Mock<IBidRepository>();
            _service = new BidService(_bidRepository.Object);
        }
    }
}
