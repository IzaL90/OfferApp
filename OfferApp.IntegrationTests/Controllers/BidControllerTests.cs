using OfferApp.Core.DTO;
using OfferApp.Core.Repositories;
using OfferApp.IntegrationTests.Common;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace OfferApp.IntegrationTests.Controllers
{
    public class BidControllerTests : BaseTest
    {
        [Fact]
        public async Task ShouldAddBid()
        {
            var bid = Fixtures.CreateBid();

            var response = await Client.PostAsJsonAsync(PATH, bid);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var content = await response.Content.ReadFromJsonAsync<BidDto>();
            content.ShouldNotBeNull();
            content.Id.ShouldBeGreaterThan(0);
            content.Name.ShouldBe(bid.Name);
        }

        [Fact]
        public async Task ShouldAddBidToDatabase()
        {
            var bid = Fixtures.CreateBid();

            var response = await Client.PostAsJsonAsync(PATH, bid);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var content = await response.Content.ReadFromJsonAsync<BidDto>();
            content.ShouldNotBeNull();
            content.Name.ShouldBe(bid.Name);
            content.Id.ShouldBeGreaterThan(0);
            var bidAdded = await _bidRepository.Get(content.Id);
            bidAdded.ShouldNotBeNull();
            bidAdded.Name.ShouldBe(bid.Name);
        }

        [Fact]
        public async Task ShouldGetBid()
        {
            var id = 1;

            var response = await Client.GetAsync($"{PATH}/{id}");

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.ShouldNotBeNull();
            var bid = await response.Content.ReadFromJsonAsync<BidDto>();
            bid.ShouldNotBeNull();
            bid.Id.ShouldBe(id);
            bid.Name.ShouldNotBeNullOrWhiteSpace();
            bid.Description.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ShouldGetAllBids()
        {
            var response = await Client.GetAsync($"{PATH}");

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.ShouldNotBeNull();
            var bids = await response.Content.ReadFromJsonAsync<IEnumerable<BidDto>>();
            bids.ShouldNotBeNull();
            bids.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ShouldUpdateBid()
        {
            var id = 1;
            var bidToUpdate = new BidDto { Id = id, Name="BidUpdated", Description="ShouldUpdateBid", FirstPrice = 10000 };

            var response = await Client.PutAsJsonAsync($"{PATH}/{id}", bidToUpdate);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var bidUpdated = await _bidRepository.Get(id);
            bidUpdated.ShouldNotBeNull();
            bidUpdated.Id.ShouldBe(id);
            bidUpdated.Name.ShouldNotBeNullOrWhiteSpace();
            bidUpdated.Name.ShouldBe(bidToUpdate.Name);
            bidUpdated.Description.ShouldNotBeNullOrWhiteSpace();
            bidUpdated.Description.ShouldBe(bidToUpdate.Description);
            bidUpdated.FirstPrice.ShouldBe(bidToUpdate.FirstPrice);
        }

        [Fact]
        public async Task ShouldDeleteBid()
        {
            var id = 2;

            var response = await Client.DeleteAsync($"{PATH}/{id}");

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var bidDeleted = await _bidRepository.Get(id);
            bidDeleted.ShouldBeNull();
        }

        private const string PATH = "api/bids";
        private readonly IBidRepository _bidRepository;

        public BidControllerTests(TestApplicationFactory testApplicationFactory) : base(testApplicationFactory)
        {
            _bidRepository = GetRequiredService<IBidRepository>();
        }
    }
}
