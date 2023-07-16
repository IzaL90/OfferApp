using OfferApp.Shared.DTO;

namespace OfferApp.Shared.Tests
{
    public static class TestData
    {
        public static IEnumerable<BidDto> GetBidsDtos()
        {
            yield return new BidDto { Id = 1, Name = "Name#1", Description = "Description#1#2023", FirstPrice = 100, Created = DateTime.Now, Count = 0 };
            yield return new BidDto { Id = 2, Name = "Name#2", Description = "Description#2#2023", FirstPrice = 200, LastPrice = 500, Created = DateTime.Now, Updated = DateTime.Now, Published = true, Count = 0 };
            yield return new BidDto { Id = 3, Name = "Name#3", Description = "Description#3#2023", FirstPrice = 300, Created = DateTime.Now, Count = 0 };
            yield return new BidDto { Id = 4, Name = "Name#4", Description = "Description#4#2023", FirstPrice = 400, LastPrice = 700, Created = DateTime.Now, Updated = DateTime.Now, Published = true, Count = 0 };
            yield return new BidDto { Id = 5, Name = "Name#5", Description = "Description#5#2023", FirstPrice = 500, Created = DateTime.Now, Count = 0 };
        }

        public static BidDto GetBidDto()
        {
            return new BidDto
            {
                Id = 1,
                Name = $"Name#{Guid.NewGuid().ToString("N")}",
                Description = $"Description#{Guid.NewGuid().ToString("N")}",
                Count = 0,
                Created = DateTime.Now,
                FirstPrice = 200
            };
        }
    }
}
