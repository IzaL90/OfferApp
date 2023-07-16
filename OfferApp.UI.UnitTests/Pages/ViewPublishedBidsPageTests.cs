using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OfferApp.Shared.DTO;
using OfferApp.Shared.Tests;
using OfferApp.UI.Pages;
using OfferApp.UI.Services;
using Shouldly;
using System.Collections.Generic;

namespace OfferApp.UI.UnitTests.Pages
{
    public class ViewPublishedBidsPageTests
    {
        [Fact]
        public void ShouldRenderPage()
        {
            var expectedText = "Hello, welcome to bid management!";

            var text = _viewPublishedBidsPage.Find("[data-name=\"offers-information\"]");

            text.ShouldNotBeNull();
            text.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            text.InnerHtml.ShouldContain(expectedText);
        }

        [Fact]
        public void GivenLoadingSetToTrue_ShouldShowSpinner()
        {
            _viewPublishedBidsPage.Instance.Loading = true;
            _viewPublishedBidsPage.Render();

            var spinner = _viewPublishedBidsPage.Find(".spinner-border");

            spinner.ShouldNotBeNull();
            spinner.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenEmptyPublishedBids_ShouldShowEmptyTable()
        {
            _viewPublishedBidsPage.Render();

            var tableBody = _viewPublishedBidsPage.Find("table > tbody");

            tableBody.ShouldNotBeNull();
            tableBody.InnerHtml.ShouldBeNullOrEmpty();
        }

        [Fact]
        public void GivenPublishedBids_ShouldShowTable()
        {
            _bidService.Setup(b => b.GetAllPublishedBids()).ReturnsAsync(TestData.GetBidsDtos().AsReadOnlyPublishedDtos());
            _viewPublishedBidsPage = _testContext.RenderComponent<ViewPublishedBidsPage>();

            var tableBody = _viewPublishedBidsPage.Find("table > tbody");

            tableBody.ShouldNotBeNull();
            tableBody.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        private IRenderedComponent<ViewPublishedBidsPage> _viewPublishedBidsPage;
        private readonly Mock<IBidService> _bidService;
        private readonly TestContext _testContext;

        public ViewPublishedBidsPageTests()
        {
            _bidService = new Mock<IBidService>();
            _testContext = new TestContext();
            _testContext.Services.AddScoped(_ => _bidService.Object);
            _viewPublishedBidsPage = _testContext.RenderComponent<ViewPublishedBidsPage>();
        }
    }
}
