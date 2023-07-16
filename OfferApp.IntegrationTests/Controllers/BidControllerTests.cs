using Microsoft.AspNetCore.Http;
using OfferApp.Shared.DTO;
using OfferApp.Domain.Entities;
using OfferApp.Domain.Repositories;
using OfferApp.IntegrationTests.Common;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using OfferApp.Shared.Tests;

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

        [Fact]
        public async Task ShouldGetAllPublished()
        {
            await AddDefaultPublishedBid();
            await AddDefaultPublishedBid();

            var response = await Client.GetAsync($"{PATH}/published");

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var publishedBids = await response.Content.ReadFromJsonAsync<IEnumerable<BidPublishedDto>>();
            publishedBids.ShouldNotBeNull();
            publishedBids.ShouldNotBeEmpty();
            publishedBids.Count().ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task ShouldIncreaseBidLastPrice()
        {
            var bid = await AddPublishedBid("Bid#2024", "DescriptionWithLongWord", 10000);
            var dto = new BidUpDto { Id = bid.Id, Price = 20000 };

            var response = await Client.PatchAsJsonAsync($"{PATH}/{bid.Id}/bid-up", dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var bidChanged = await response.Content.ReadFromJsonAsync<BidPublishedDto>();
            bidChanged.ShouldNotBeNull();
            bidChanged.LastPrice.ShouldNotBeNull();
            bidChanged.LastPrice.Value.ShouldBe(dto.Price);
            var bidUpdated = await _bidRepository.Get(bid.Id);
            bidUpdated.ShouldNotBeNull();
            bidUpdated.LastPrice.ShouldNotBeNull();
            bidUpdated.LastPrice.Value.ShouldBe(dto.Price);
        }

        [Fact]
        public async Task ShouldPublishBid()
        {
            var id = 5;

            var response = await Client.PatchAsync($"{PATH}/{id}/publish", null);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var bidUpdated = await _bidRepository.Get(id);
            bidUpdated.ShouldNotBeNull();
            bidUpdated.Published.ShouldBeTrue();
        }

        [Fact]
        public async Task ShouldUnpublishBid()
        {
            var bid = await AddDefaultPublishedBid();

            var response = await Client.PatchAsync($"{PATH}/{bid.Id}/unpublish", null);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var bidUpdated = await _bidRepository.Get(bid.Id);
            bidUpdated.ShouldNotBeNull();
            bidUpdated.Published.ShouldBeFalse();
        }

        [Fact]
        public async Task GivenInvalidId_WhenGetBid_ShouldReturnNotFound()
        {
            var id = 2000;

            var response = await Client.GetAsync($"{PATH}/{id}");

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenInvalidId_WhenUpdateBid_ShouldReturnNotFound()
        {
            var bid = new BidDto { Id = 2000, Name = "Name#1", Description = "Description#2024", FirstPrice = 2000 };

            var response = await Client.PutAsJsonAsync($"{PATH}/{bid.Id}", bid);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenInvalidPayload_WhenAddNewBid_ShouldReturnBadRequest()
        {
            var dto = new BidDto() { FirstPrice = -200 };

            var response = await Client.PostAsJsonAsync($"{PATH}", dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var errorModel = await response.Content.ReadFromJsonAsync<ErrorModel>();
            errorModel.ShouldNotBeNull();
            errorModel.Status.ShouldBe(StatusCodes.Status400BadRequest);
            errorModel.Errors.ShouldNotBeNull();
            errorModel.Errors.ShouldNotBeEmpty();
            errorModel.Errors["Name"].ShouldNotBeNull();
            errorModel.Errors["Name"].ShouldNotBeEmpty();
            errorModel.Errors["Description"].ShouldNotBeNull();
            errorModel.Errors["Description"].ShouldNotBeEmpty();
            errorModel.Errors["FirstPrice"].ShouldNotBeEmpty();
            errorModel.Errors["FirstPrice"].ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenPublishedBid_WhenUnpublish_ShouldResetBid()
        {
            var bid = await AddDefaultPublishedBid();
            var dto = new BidUpDto() { Id = bid.Id, Price = bid.FirstPrice + 20000 };
            await BidUp(dto);

            var response = await Client.PatchAsync($"{PATH}/{bid.Id}/unpublish", null);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var bidUpdated = await _bidRepository.Get(bid.Id);
            bidUpdated.ShouldNotBeNull();
            bidUpdated.Published.ShouldBeFalse();
            bidUpdated.LastPrice.HasValue.ShouldBeFalse();
            bidUpdated.Updated.HasValue.ShouldBeFalse();
            bidUpdated.Count.ShouldBe(0);
        }

        private async Task BidUp(BidUpDto dto)
        {
            var response = await Client.PatchAsJsonAsync($"{PATH}/{dto.Id}/bid-up", dto);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private async Task<Bid> AddPublishedBid(string name, string description, decimal firstPrice)
        {
            var bid = Bid.Create(name, description, firstPrice);
            bid.Publish();
            await _bidRepository.Add(bid);
            return bid;
        }

        private async Task<Bid> AddDefaultPublishedBid()
        {
            var bid = Fixtures.CreatePublishedBid();
            await _bidRepository.Add(bid);
            return bid;
        }

        private const string PATH = "api/bids";
        private readonly IBidRepository _bidRepository;

        public BidControllerTests(TestApplicationFactory testApplicationFactory) : base(testApplicationFactory)
        {
            _bidRepository = GetRequiredService<IBidRepository>();
        }
    }
}
