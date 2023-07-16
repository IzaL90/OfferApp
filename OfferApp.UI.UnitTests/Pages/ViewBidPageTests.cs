using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharpWrappers;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OfferApp.Shared.Tests;
using OfferApp.UI.Pages;
using OfferApp.UI.Services;
using Shouldly;

namespace OfferApp.UI.UnitTests.Pages
{
    public class ViewBidPageTests
    {
        [Fact]
        public void ShouldRenderPage()
        {
            var text = _viewBidPage.Find("[data-name=\"bid-not-found-message\"]");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenLoadingSetToTrue_ShouldShowSpinner()
        {
            _viewBidPage.Instance.Loading = true;
            _viewBidPage.Render();

            var spinner = _viewBidPage.Find(".spinner-border");

            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenBid_ShoulShowDetails()
        {
            var bid = TestData.GetBidDto();
            _bidService.Setup(b => b.GetBidById(It.IsAny<int>())).ReturnsAsync(bid);
           
            _viewBidPage = _testContext.RenderComponent<ViewBidPage>();

            var name = _viewBidPage.Find("[data-name=\"bid-name-header-text\"]");
            name.ShouldNotBeNull();
            name.InnerHtml.ShouldContain(bid.Name);
            var description = _viewBidPage.Find("[data-name=\"bid-description-input\"]");
            description.ShouldNotBeNull();
            description.InnerHtml.ShouldContain(bid.Description);
            var firstPrice = (_viewBidPage.Find("[data-name=\"bid-first-price-input\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            firstPrice.ShouldNotBeNull();
            firstPrice.Value.ShouldBe(bid.FirstPrice.ToString());            
            var created = (_viewBidPage.Find("[data-name=\"bid-created-input\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            created.ShouldNotBeNull();
            created.Value.ShouldContain(bid.Created.ToString());
            var count = (_viewBidPage.Find("[data-name=\"bid-count-input\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            count.ShouldNotBeNull();
            count.Value.ShouldContain(bid.Count.ToString());
            var published = (_viewBidPage.Find("[data-name=\"bid-published-input\"]") as ElementWrapper)?.WrappedElement as IHtmlInputElement;
            published.ShouldNotBeNull();
            published.Value.ShouldContain(bid.Published ? "Yes" : "No");
        }

        private IRenderedComponent<ViewBidPage> _viewBidPage;
        private readonly Mock<IBidService> _bidService;
        private readonly TestContext _testContext;

        public ViewBidPageTests()
        {
            _bidService = new Mock<IBidService>();
            _testContext = new TestContext();
            _testContext.Services.AddScoped(_ => _bidService.Object);
            _viewBidPage = _testContext.RenderComponent<ViewBidPage>();
        }
    }
}
