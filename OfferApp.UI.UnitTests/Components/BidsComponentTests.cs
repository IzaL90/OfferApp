using AngleSharp.Dom;
using Bunit;
using OfferApp.Shared.DTO;
using OfferApp.Shared.Tests;
using OfferApp.UI.Components;
using Shouldly;

namespace OfferApp.UI.UnitTests.Components
{
    public class BidsComponentTests
    {
        [Fact]
        public void ShouldRenderComponent()
        {
            var table = _component.Find("table");
            table.ShouldNotBeNull();
            table.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void ClickedOnRow_ShouldSetBidClicked()
        {
            var bid = _bids.First();

            ClickOnBid(bid);

            _component.Instance.BidClicked.ShouldNotBeNull();
            _component.Instance.BidClicked.Id.ShouldBe(bid.Id);
        }

        [Fact]
        public void ClickedRowOnPublishBid_ShouldShowButtonChangingBidToUnpublish()
        {
            var bidPublished = _bids.First(b => b.Published);
            
            ClickOnBid(bidPublished);

             var unpublishButton = _component.Find("[data-name=\"bid-set-as-unpublish-button\"]");
            unpublishButton.ShouldNotBeNull();
        }

        [Fact]
        public void ClickedRowOnUnpublishBid_ShouldShowButtonChangingBidToPublish()
        {
            var bidUnpublished = _bids.First(b => !b.Published);

            ClickOnBid(bidUnpublished);

            var publishButton = _component.Find("[data-name=\"bid-set-as-publish-button\"]");
            publishButton.ShouldNotBeNull();
        }

        [Fact]
        public void UnpublishBid_ShouldEmitEventSetBidPublish()
        {
            SetBidPublishDto? bidToPublish = null;
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bids, _bids);
                p.Add(param => param.OnPublish, b => bidToPublish = b);
            });
            var bidPublished = _bids.First(b => b.Published);
            ClickOnBid(bidPublished);

            SetBidPublish(false);

            bidToPublish.ShouldNotBeNull();
            bidToPublish.Id.ShouldBe(bidToPublish.Id);
        }

        [Fact]
        public void PublishBid_ShouldEmitEventSetBidPublish()
        {
            SetBidPublishDto? bidToPublish = null;
            _component.SetParametersAndRender(p =>
            {
                p.Add(param => param.Bids, _bids);
                p.Add(param => param.OnPublish, b => bidToPublish = b);
            });
            var bidUnpublished = _bids.First(b => !b.Published);
            ClickOnBid(bidUnpublished);

            SetBidPublish(true);

            bidToPublish.ShouldNotBeNull();
            bidToPublish.Id.ShouldBe(bidToPublish.Id);
        }

        [Fact]
        public void ClickedOnDeleteButton_ShouldSetBidToDelete()
        {
            var bid = _bids.First();
            
            ClickOnDeleteButton(bid);

            _component.Instance.BidToDelete.ShouldNotBeNull();
            _component.Instance.BidToDelete.Id.ShouldBe(bid.Id);
        }

        [Fact]
        public void ClickedOnDeleteButton_ShouldShowModal()
        {
            var bid = _bids.First();
            
            ClickOnDeleteButton(bid);

            var modal = _component.Find("#myModal");
            modal.ShouldNotBeNull();
            modal.InnerHtml.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void DeletedBid_ShouldEmitEventOnDelete()
        {
            BidDto? bidToDelete = null;
            _component.SetParametersAndRender(b =>
            {
                b.Add(param => param.Bids, _bids);
                b.Add(param => param.OnDelete, b => bidToDelete = b);
            });
            var bid = _bids.First();
            ClickOnDeleteButton(bid);

            DeleteBid();

            bidToDelete.ShouldNotBeNull();
            bidToDelete.Id.ShouldBe(bid.Id);
        }

        [Fact]
        public void GenericExceptionOccured_WhenDeleteBid_ShouldGenericShowInformation()
        {
            _component.SetParametersAndRender(b =>
            {
                b.Add(param => param.Bids, _bids);
                b.Add(param => param.OnDelete, () => throw new Exception());
            });
            var bid = _bids.First();
            ClickOnDeleteButton(bid);

            DeleteBid();

            var error = FindError();
            error.InnerHtml.ShouldContain("Something bad happen, please try again later");
        }

        [Fact]
        public void InvalidOperationOccured_WhenDeleteBid_ShouldShowInformation()
        {
            var exception = new InvalidOperationException("Bid cannot be deleted");
            _component.SetParametersAndRender(b =>
            {
                b.Add(param => param.Bids, _bids);
                b.Add(param => param.OnDelete, () => throw exception);
            });
            var bid = _bids.First();
            ClickOnDeleteButton(bid);

            DeleteBid();

            var error = FindError();
            error.InnerHtml.ShouldContain(exception.Message);
        }

        [Fact]
        public void GivenPublishedBid_WhenDeleteBid_ShouldInformCannotDeleteBid()
        {
            var bid = _bids.First(b => b.Published);
            ClickOnDeleteButton(bid);

            DeleteBid();

            var error = FindError();
            error.InnerHtml.ShouldContain("Cannot delete published offer");
        }

        private IElement FindError()
        {
            var error = _component.Find("[data-name=\"error-value\"]");
            error.ShouldNotBeNull();
            return error;
        }

        private void DeleteBid()
        {
            var button = _component.Find("[data-name=\"bid-delete-action-confirm\"]");
            button.ShouldNotBeNull();
            button.Click();
        }

        private void ClickOnDeleteButton(BidDto bid)
        {
            var button = _component.Find($"[data-name=\"bid-{bid.Id}-delete-action\"]");
            button.ShouldNotBeNull();
            button.Click();
        }

        private void ClickOnBid(BidDto bid)
        {
            var elementRow = _component.Find($"[data-name=\"bid-row-action-{bid.Id}\"]");
            elementRow.ShouldNotBeNull();
            elementRow.Click();
        }

        private void SetBidPublish(bool publish)
        {
            IElement element;
            if (publish)
            {
                element = _component.Find("[data-name=\"bid-set-as-publish-button\"]");
            }
            else
            {
                element = _component.Find("[data-name=\"bid-set-as-unpublish-button\"]");
            }
            element.ShouldNotBeNull();
            element.Click();
        }

        private readonly IRenderedComponent<BidsComponent> _component;
        private readonly IEnumerable<BidDto> _bids;

        public BidsComponentTests()
        {
            _bids = TestData.GetBidsDtos();
            var testContext = new TestContext();
            _component = testContext.RenderComponent<BidsComponent>(p =>
            {
                p.Add(param => param.Bids, _bids);
            });
        }
    }
}
