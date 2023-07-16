using Bunit;
using Microsoft.AspNetCore.Components;
using OfferApp.UI.Components;
using Shouldly;

namespace OfferApp.UI.UnitTests.Components
{
    public class ModalComponentTests
    {
        [Fact]
        public void ShouldRenderComponent()
        {
            var modal = _component.Find("#myModal");
            modal.ShouldNotBeNull();
            modal.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenTitle_ShouldRenderOnModal()
        {
            var title = "Title#1";

            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Title, title);
            });

            var modalTitle = _component.Find("[data-name=\"modal-title\"]");
            modalTitle.ShouldNotBeNull();
            modalTitle.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            modalTitle.InnerHtml.ShouldContain(title);
        }

        [Fact]
        public void GivenHtmlTags_ShouldRenderComponentWithExpectedHtml()
        {
            var title = "Title#1";
            var expectedHtml = """
                <div class="container"><span>This is text</span></div>
                """;
            RenderFragment fragment = builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "container");
                builder.OpenElement(2, "span");
                builder.AddContent(3, "This is text");
                builder.CloseElement();
                builder.CloseElement();
            };

            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Title, title);
                p.Add(param => param.Content, fragment);
            });

            var content = _component.Find(".modal-content");
            content.ShouldNotBeNull();
            content.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            content.InnerHtml.ShouldContain(expectedHtml);
        }

        private readonly IRenderedComponent<ModalComponent> _component;

        public ModalComponentTests()
        {
            var testContext = new TestContext();
            _component = testContext.RenderComponent<ModalComponent>();
        }
    }
}
