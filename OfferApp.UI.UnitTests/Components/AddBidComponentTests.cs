using Bunit;
using OfferApp.Shared.DTO;
using OfferApp.Shared.Tests;
using OfferApp.UI.Components;
using Shouldly;

namespace OfferApp.UI.UnitTests.Components
{
    public class AddBidComponentTests
    {
        [Fact]
        public void ShouldRenderComponent()
        {
            _component.ShouldNotBeNull();
            _component.Instance.ShouldNotBeNull();
            var addButton = _component.Find("[data-name=\"bid-add-button\"]");
            addButton.ShouldNotBeNull();
            addButton.InnerHtml.ShouldNotBeNullOrWhiteSpace();
            addButton.InnerHtml.ShouldContain("Add");
        }

        [Fact]
        public void TriggerAddButton_ShouldRenderModalWithAddingNewBid()
        {
            RenderModal();

            var modal = _component.Find("#myModal");
            modal.ShouldNotBeNull();
        }

        [Fact]
        public void AddInvalidBid_ShouldShowValidationErrors()
        {
            RenderModal();

            SendForm();

            var validationMessages = _component.FindAll(".validation-message");
            validationMessages.ShouldNotBeEmpty();
        }

        [Fact]
        public void AddValidBid_ShouldSendEventOnAddAndCloseModal()
        {
            BidDto? addedBid = null;
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.OnAdd, a => addedBid = a);
            });
            RenderModal();
            var bid = Fixtures.CreateBid();
            FillForm(new BidDto { Name = bid.Name, FirstPrice = bid.FirstPrice, Description = bid.Description });

            SendForm();

            var exception = Record.Exception(() => _component.Find("#myModal"));
            exception.ShouldNotBeNull();
            addedBid.ShouldNotBeNull();
            addedBid.Name.ShouldBe(bid.Name);
            addedBid.FirstPrice.ShouldBe(bid.FirstPrice);
            addedBid.Description.ShouldBe(bid.Description);
        }

        private void FillForm(BidDto dto)
        {
            var bidName = _component.Find("[data-name=\"bid-name-input\"]");
            bidName.ShouldNotBeNull();
            bidName.Change(dto.Name);
            var firstPrice = _component.Find("[data-name=\"bid-first-price-input\"]");
            firstPrice.ShouldNotBeNull();
            firstPrice.Change(dto.FirstPrice);
            var description = _component.Find("[data-name=\"bid-description-input\"]");
            description.ShouldNotBeNull();
            description.Change(dto.Description);
        }

        private void SendForm()
        {
            var submitButton = _component.Find("[data-name=\"bid-submit-button\"]");
            submitButton.ShouldNotBeNull();
            submitButton.Click();
        }

        private void RenderModal()
        {
            var addButton = _component.Find("[data-name=\"bid-add-button\"]");
            addButton.ShouldNotBeNull();
            addButton.Click();
        }

        private readonly IRenderedComponent<AddBidComponent> _component;

        public AddBidComponentTests()
        {
            var testContext = new TestContext();
            _component = testContext.RenderComponent<AddBidComponent>();
        }
    }
}
