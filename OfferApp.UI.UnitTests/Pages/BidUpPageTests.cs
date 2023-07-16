using AngleSharp.Html.Dom;
using AngleSharpWrappers;
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
    public class BidUpPageTests
    {
        [Fact]
        public void ShouldRenderPage()
        {
            var expectedText = "Bid Up";

            var text = _bidUpPage.Find("h3");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            text.InnerHtml.ShouldContain(expectedText);
        }

        [Fact]
        public void GivenLoadingSetToTrue_ShouldShowSpinner()
        {
            _bidUpPage.Instance.Loading = true;
            _bidUpPage.Render();

            var spinner = _bidUpPage.Find(".spinner-border");

            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenNullBid_ShouldShowInformation()
        {
            var information = _bidUpPage.Find("[data-name=\"bid-not-found-message\"]");
            information.ShouldNotBeNull();
            information.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenNotPublishedBid_ShouldShowInformation()
        {
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(TestData.GetBidDto());
            
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();

            var information = _bidUpPage.Find("[data-name=\"bid-not-published-message\"]");
            information.ShouldNotBeNull();
            information.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenPublishedBid_ShouldAllowToBidUp()
        {
            var bid = TestData.GetBidDto();
            bid.Published = true;
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(bid);
            
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();

            var information = _bidUpPage.Find("[data-name=\"bid-up-form\"]");
            information.ShouldNotBeNull();
            information.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            var bidUpInputValue = (_bidUpPage.Find("[data-name=\"bid-up-input-value\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            bidUpInputValue.ShouldNotBeNull();
            bidUpInputValue.Value.ShouldBe(bid.FirstPrice.ToString());
        }

        [Fact]
        public void GivenPublishedBidWithLastPrice_ShouldAllowToBidUp()
        {
            var bid = TestData.GetBidDto();
            bid.Published = true;
            bid.LastPrice = bid.FirstPrice + 20000;
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(bid);
            
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();

            var information = _bidUpPage.Find("[data-name=\"bid-up-form\"]");
            information.ShouldNotBeNull();
            information.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            var bidUpInputValue = (_bidUpPage.Find("[data-name=\"bid-up-input-value\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            bidUpInputValue.ShouldNotBeNull();
            bidUpInputValue.Value.ShouldBe(bid.LastPrice.ToString());
        }

        [Fact]
        public void GivenPublishedBid_WhenBidUp_ShouldSendValue()
        {
            var bid = TestData.GetBidDto();
            bid.Published = true;
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(bid);
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();
            var priceToBidUp = bid.FirstPrice + (bid.LastPrice ?? 0) + 100;
            BidUp(priceToBidUp);

            Submit();

            _bidService.Verify(b => b.BidUp(It.IsAny<BidUpDto>()), times: Times.Once);
            var exception = Record.Exception(() => _bidUpPage.Find("[data-name=\"error-value\"]"));
            exception.ShouldNotBeNull();
            exception.Message.Contains("No elements were found that matches the selector");
        }

        [Fact]
        public void GivenPublishedBidAnErrorOccured_WhenBidUp_ShouldShowError()
        {
            var bid = TestData.GetBidDto();
            bid.Published = true;
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(bid);
            _bidService.Setup(b => b.BidUp(It.IsAny<BidUpDto>())).Throws(new Exception("error"));
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();
            var priceToBidUp = bid.FirstPrice - 10;
            BidUp(priceToBidUp);

            Submit();

            _bidService.Verify(b => b.BidUp(It.IsAny<BidUpDto>()), times: Times.Once);
            var error = _bidUpPage.Find("[data-name=\"error-value\"]");
            error.ShouldNotBeNull();
            error.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        private void BidUp(decimal value)
        {
            var priceToBidUp = _bidUpPage.Find("[data-name=\"bid-up-input-value\"]");
            priceToBidUp.ShouldNotBeNull();
            priceToBidUp.Change(value);
        }

        private void Submit()
        {
            var submitButton = _bidUpPage.Find("[data-name=\"bid-up-button-accept\"]");
            submitButton.Click();
        }

        private IRenderedComponent<BidUpPage> _bidUpPage;
        private readonly Mock<IBidService> _bidService;
        private readonly TestContext _testContext;

        public BidUpPageTests()
        {
            _bidService = new Mock<IBidService>();
            _testContext = new TestContext();
            _testContext.Services.AddScoped(_ => _bidService.Object);
            _bidUpPage = _testContext.RenderComponent<BidUpPage>();
        }
    }
}
