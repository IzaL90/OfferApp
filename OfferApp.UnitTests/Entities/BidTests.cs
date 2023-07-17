using OfferApp.Domain.Entities;
using OfferApp.Domain.Exceptions;
using OfferApp.Shared.Tests;
using Shouldly;

namespace OfferApp.UnitTests.Entities
{
    public class BidTests
    {
        [Fact]
        public void ShouldCreateBid()
        {
            var name = "name";
            var description = "description-test12345";
            var firstPrice = 100;

            var bid = Bid.Create(name, description, firstPrice);

            bid.ShouldNotBeNull();
            bid.Name.ShouldBe(name);
            bid.Description.ShouldBe(description);
            bid.FirstPrice.ShouldBe(firstPrice);
        }

        [Theory]
        [InlineData("           ")]
        [InlineData(null)]
        [InlineData("")]
        public void GivenInvalidName_WhenCreateBid_ShouldThrowAnException(string? name)
        {
            var description = "description-test12345";
            var firstPrice = 100;

            var exception = Record.Exception(() => Bid.Create(name, description, firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Name cannot be empty");
        }

        [Fact]
        public void GivenTooShortName_WhenCreateBid_ShouldThrowAnException()
        {
            var name = "abc";
            var description = "description-test12345";
            var firstPrice = 100;

            var exception = Record.Exception(() => Bid.Create(name, description, firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("at least 4 characters");
        }

        [Fact]
        public void GivenPublishedBid_WhenChangeBidName_ShouldThrowAnException()
        {
            var name = "abc12345";
            var bid = Fixtures.CreatePublishedBid();

            var exception = Record.Exception(() => bid.ChangeName(name));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Cannot change an offer once it has been published");
        }

        [Fact]
        public void ShouldChangeBidName()
        {
            var bid = Fixtures.CreateBid();
            var name = "test12345";

            bid.ChangeName(name);

            bid.Name.ShouldBe(name);
        }

        [Theory]
        [InlineData("           ")]
        [InlineData(null)]
        [InlineData("")]
        public void GivenInvalidDescription_WhenCreateBid_ShouldThrowAnException(string? description)
        {
            var name = "name-test12345";
            var firstPrice = 100;

            var exception = Record.Exception(() => Bid.Create(name, description, firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Description cannot be empty");
        }

        [Fact]
        public void GivenTooDescriptionName_WhenCreateBid_ShouldThrowAnException()
        {
            var name = "abc1234";
            var description = "descripti";
            var firstPrice = 100;

            var exception = Record.Exception(() => Bid.Create(name, description, firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("should contain at least 10 characters");
        }

        [Fact]
        public void GivenPublishedBid_WhenChangeBidDescription_ShouldThrowAnException()
        {
            var description = "description12345";
            var bid = Fixtures.CreatePublishedBid();

            var exception = Record.Exception(() => bid.ChangeDescription(description));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Cannot change an offer once it has been published");
        }

        [Fact]
        public void ShouldChangeBidDescription()
        {
            var bid = Fixtures.CreateBid();
            var description = "abc12345-test12345";

            bid.ChangeDescription(description);

            bid.Description.ShouldBe(description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void GivenNegativeFirstPrice_WhenCreateBid_ShouldThrowAnException(decimal firstPrice)
        {
            var name = "name";
            var description = "description-test12345";

            var exception = Record.Exception(() => Bid.Create(name, description, firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("cannot be negative");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void GivenNegativeFirstPrice_WhenChangeBidFirstPrice_ShouldThrowAnException(decimal firstPrice)
        {
            var bid = Fixtures.CreateBid();

            var exception = Record.Exception(() => bid.ChangeFirstPrice(firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("cannot be negative");
        }

        [Fact]
        public void GivenPublishedBid_WhenChangeBidFirstPrice_ShouldThrowAnException()
        {
            var firstPrice = 200000;
            var bid = Fixtures.CreatePublishedBid();

            var exception = Record.Exception(() => bid.ChangeFirstPrice(firstPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Cannot change an offer once it has been published");
        }

        [Fact]
        public void ShouldChangeBidFirstPrice()
        {
            var firstPrice = 200000;
            var bid = Fixtures.CreateBid();

            bid.ChangeFirstPrice(firstPrice);

            bid.FirstPrice.ShouldBe(firstPrice);
        }

        [Fact]
        public void GivenNotPublishedBid_WhenChangePrice_ShouldThrowAnException()
        {
            var bid = Fixtures.CreateBid();

            var exception = Record.Exception(() => bid.ChangePrice(100));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain("Cannot change an offer once it hasn't been published");
        }

        [Fact]
        public void GivenPublishedBidWithFirstPriceAndValueLessThanFirstPrice_WhenChangePrice_ShouldThrowAnException()
        {
            var bid = Fixtures.CreatePublishedBid();

            var exception = Record.Exception(() => bid.ChangePrice(bid.FirstPrice - 1));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain($"cannot be less than '{bid.FirstPrice}'");
        }

        [Fact]
        public void GivenPublishedBidWithValueLessThanLastPrice_WhenChangePrice_ShouldThrowAnException()
        {
            var bid = Fixtures.CreatePublishedBid();
            bid.ChangePrice(100000);

            var exception = Record.Exception(() => bid.ChangePrice(10));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain($"cannot be less than '{bid.LastPrice}'");
        }

        [Fact]
        public void GivenPublishedBidWithValueEqualsLastPrice_WhenChangePrice_ShouldThrowAnException()
        {
            var bid = Fixtures.CreatePublishedBid();
            var value = 100000;
            bid.ChangePrice(value);

            var exception = Record.Exception(() => bid.ChangePrice(value));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OfferException>();
            exception.Message.ShouldContain($"cannot be same as '{value}'");
        }

        [Fact]
        public void GivenPublishedBid_WhenChangePrice_ShouldSetNewLastPrice()
        {
            var bid = Fixtures.CreatePublishedBid();
            var price = 100000;
            var dateBeforeChange = DateTime.UtcNow;

            bid.ChangePrice(price);

            bid.LastPrice.ShouldBe(price);
            bid.Count.ShouldBe(1);
            bid.Updated.HasValue.ShouldBeTrue();
            bid.Updated.Value.ShouldBeGreaterThan(dateBeforeChange);
            bid.Updated.Value.ShouldBeLessThan(DateTime.UtcNow);
        }

        [Fact]
        public void GivenPublishedBidWithSetLastPrice_WhenChangePrice_ShouldSetNewLastPrice()
        {
            var bid = Fixtures.CreatePublishedBid();
            var price = 100000;
            bid.ChangePrice(price);
            var newPrice = price + 10;
            var dateBeforeChange = DateTime.UtcNow;

            bid.ChangePrice(newPrice);

            bid.LastPrice.ShouldBe(newPrice);
            bid.Count.ShouldBe(2);
            bid.Updated.HasValue.ShouldBeTrue();
            bid.Updated.Value.ShouldBeGreaterThan(dateBeforeChange);
            bid.Updated.Value.ShouldBeLessThan(DateTime.UtcNow);
        }

        [Fact]
        public void GivenPublishedBid_WhenUnpublish_Should_ClearLastPriceUpdatedAndCount()
        {
            var bid = Fixtures.CreatePublishedBid();
            bid.ChangePrice(100000);
            var lastCount = bid.Count;

            bid.Unpublish();

            bid.LastPrice.HasValue.ShouldBeFalse();
            bid.Count.ShouldBeLessThan(lastCount);
            bid.Count.ShouldBe(0);
            bid.Updated.HasValue.ShouldBeFalse();
        }
    }
}