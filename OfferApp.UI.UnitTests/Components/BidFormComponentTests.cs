using Bunit;
using OfferApp.Shared.DTO;
using OfferApp.UI.Components;
using Shouldly;

namespace OfferApp.UI.UnitTests.Components
{
    public class BidFormComponentTests
    {
        [Fact]
        public void ShouldRenderBidForm()
        {
            var bidName = _component.Find("[data-name=\"bid-name-input\"]");
            bidName.ShouldNotBeNull();
        }

        [Fact]
        public void GivenInvalidBid_ShouldShowValidationErrors()
        {
            var dto = new BidDto { Name = "a", Description = "b", FirstPrice = -10 };
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bid, dto);
            });

            SubmitForm();

            var validationMessages = _component.FindAll(".validation-message");
            validationMessages.ShouldNotBeEmpty();
            validationMessages.Any(v => v.InnerHtml.Contains("Name")).ShouldBeTrue();
            validationMessages.Any(v => v.InnerHtml.Contains("Description")).ShouldBeTrue();
            validationMessages.Any(v => v.InnerHtml.Contains("First Price")).ShouldBeTrue();
        }

        [Fact]
        public void GivenValidBid_ShouldTriggerOnAddCallback()
        {
            BidDto? addedBid = null;
            var dto = new BidDto { Name = "Name#12345", Description = "Description#2023", FirstPrice = 10 };
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bid, dto);
                p.Add(param=>param.OnAdd, b => addedBid = b);
            });

            SubmitForm();

            var validationMessages = _component.FindAll(".validation-message");
            validationMessages.ShouldBeEmpty();
            addedBid.ShouldNotBeNull();
            addedBid.Name.ShouldBe(dto.Name);
            addedBid.FirstPrice.ShouldBe(dto.FirstPrice);
            addedBid.Description.ShouldBe(dto.Description);
        }

        [Fact]
        public async Task GivenInvalidBid_WhenSendForm_ShouldntTriggerOnAddCallback()
        {
            BidDto? addedBid = null;
            var dto = new BidDto { Name = "a", Description = "b", FirstPrice = -10 };
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bid, dto);
                p.Add(param=>param.OnAdd, b => addedBid = b);
            });

            await _component.Instance.Send();
            
            addedBid.ShouldBeNull();
        }

        [Fact]
        public async Task GivenValidBid_WhenSendForm_ShouldTriggerOnAddCallback()
        {
            BidDto? addedBid = null;
            var dto = new BidDto { Name = "Name#12345", Description = "Description#2023", FirstPrice = 10 };
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bid, dto);
                p.Add(param=>param.OnAdd, b => addedBid = b);
            });

            await _component.Instance.Send();
            
            addedBid.ShouldNotBeNull();
            addedBid.Name.ShouldBe(dto.Name);
            addedBid.FirstPrice.ShouldBe(dto.FirstPrice);
            addedBid.Description.ShouldBe(dto.Description);
        }

        private void SubmitForm()
        {
            var form = _component.Find("form");
            form.ShouldNotBeNull();
            form.Submit();
        }

        private readonly IRenderedComponent<BidFormComponent> _component;

        public BidFormComponentTests()
        {
            var testContext = new TestContext();
            _component = testContext.RenderComponent<BidFormComponent>();
        }
    }
}
