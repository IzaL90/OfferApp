using FluentValidation;

namespace OfferApp.Shared.DTO
{
    public class BidUpDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }

    public class BidUpDtoValidator : AbstractValidator<BidUpDto> 
    {
        public BidUpDtoValidator()
        {
            RuleFor(b => b.Price).GreaterThan(0);
        }
    }
}
