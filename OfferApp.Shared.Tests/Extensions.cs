using OfferApp.Shared.DTO;

namespace OfferApp.Shared.Tests
{
    public static class Extensions
    {
        public static BidPublishedDto ToPublished(this BidDto dto)
        {
            return new BidPublishedDto()
            {
                Id = dto.Id,
                Name = dto.Name,
                LastPrice = dto.LastPrice,
                FirstPrice = dto.FirstPrice,
                Description = dto.Description,
                Created = dto.Created,
                Count = dto.Count,
                Updated = dto.Updated
            };
        }

        public static IReadOnlyList<BidPublishedDto> AsReadOnlyPublishedDtos(this IEnumerable<BidDto> dtos)
        {
            return dtos.Select(b => b.ToPublished()).ToList();
        }
    }
}
