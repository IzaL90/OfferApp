using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OfferApp.Shared.DTO;
using OfferApp.Shared.Tests;
using OfferApp.UI.Pages;
using OfferApp.UI.Services;
using Shouldly;

namespace OfferApp.UI.UnitTests.Pages
{
    public class IndexPageTests
    {
        [Fact]
        public void ShouldRenderPage()
        {
            var expectedText = "Hello, welcome to bid management!";

            var text = _indexPage.Find("[data-name=\"offers-information\"]");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            text.InnerHtml.ShouldContain(expectedText);
        }

        [Fact]
        public void GivenBids_ShouldShowTable()
        {
            _bidService.Setup(b => b.GetAllBids()).ReturnsAsync(TestData.GetBidsDtos().ToList());

            _indexPage = _testContext.RenderComponent<IndexPage>();

            var tableBody = _indexPage.Find("table > tbody");
            tableBody.ShouldNotBeNull();
            tableBody.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenBids_WhenDelete_ShouldInvokeDelete()
        {
            var bids = TestData.GetBidsDtos().ToList();
            _bidService.Setup(b => b.GetAllBids()).ReturnsAsync(bids);
            _indexPage = _testContext.RenderComponent<IndexPage>();
            var bid = bids[0];
            var deleteButton = _indexPage.Find($"[data-name=\"bid-{bid.Id}-delete-action\"]");
            deleteButton.Click();
            var deleteConfirmation = _indexPage.Find("[data-name=\"bid-delete-action-confirm\"]");

            deleteConfirmation.Click();

            _bidService.Verify(b => b.DeleteBid(bid.Id), times: Times.Once);
        }

        [Fact]
        public void GivenBids_WhenAdd_ShouldInvokeAdd()
        {
            _bidService.Setup(b => b.GetAllBids()).ReturnsAsync(TestData.GetBidsDtos().ToList());
            _indexPage = _testContext.RenderComponent<IndexPage>();
            var addButton = _indexPage.Find("[data-name=\"bid-add-button\"]");
            addButton.Click();
            FillForm(new BidDto { Name = "Name#1", Description = "Description#1", FirstPrice = 100 });
            var submitButton = _indexPage.Find("[data-name=\"bid-submit-button\"]");
            
            submitButton.Click();

            _bidService.Verify(b => b.AddBid(It.IsAny<BidDto>()), times: Times.Once);
        }

        private void FillForm(BidDto dto)
        {
            var bidName = _indexPage.Find("[data-name=\"bid-name-input\"]");
            bidName.ShouldNotBeNull();
            bidName.Change(dto.Name);
            var firstPrice = _indexPage.Find("[data-name=\"bid-first-price-input\"]");
            firstPrice.ShouldNotBeNull();
            firstPrice.Change(dto.FirstPrice);
            var description = _indexPage.Find("[data-name=\"bid-description-input\"]");
            description.ShouldNotBeNull();
            description.Change(dto.Description);
        }

        private IRenderedComponent<IndexPage> _indexPage;
        private readonly Mock<IBidService> _bidService;
        private readonly TestContext _testContext;

        public IndexPageTests()
        {
            _bidService = new Mock<IBidService>();
            _testContext = new TestContext();
            _testContext.Services.AddScoped(_ => _bidService.Object);
            _indexPage = _testContext.RenderComponent<IndexPage>();
        }
    }
}