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
    public class EditBidPageTests
    {
        [Fact]
        public void ShouldRenderPage()
        {
            var expectedText = "Edit Bid";

            var text = _editBidPage.Find("h3");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            text.InnerHtml.ShouldContain(expectedText);
        }

        [Fact]
        public void GivenLoadingSetToTrue_ShouldShowSpinner()
        {
            _editBidPage.Instance.Loading = true;
            _editBidPage.Render();

            var spinner = _editBidPage.Find(".spinner-border");

            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenNoBid_ShouldShowInformation()
        {
            var text = _editBidPage.Find("[data-name=\"bid-not-found-message\"]");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenBid_ShoulShowForm()
        {
            var bid = TestData.GetBidDto();
            _editBidPage.Instance.Bid = bid;
            
            _editBidPage.Render();

            var form = _editBidPage.Find("form");
            form.ShouldNotBeNull();
            form.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenValidBid_ShouldUpdate()
        {
            var bid = TestData.GetBidDto();
            _editBidPage.Instance.Bid = bid;
            _editBidPage.Render();
            var updateButton = _editBidPage.Find("[data-name=\"bid-submit-button\"]");

            updateButton.Click();

            _bidService.Verify(b => b.UpdateBid(It.IsAny<BidDto>()), times: Times.Once);
        }

        private readonly IRenderedComponent<EditBidPage> _editBidPage;
        private readonly Mock<IBidService> _bidService;

        public EditBidPageTests()
        {
            _bidService = new Mock<IBidService>();
            var testContext = new TestContext();
            testContext.Services.AddScoped(_ => _bidService.Object);
            _editBidPage = testContext.RenderComponent<EditBidPage>();
        }
    }
}
