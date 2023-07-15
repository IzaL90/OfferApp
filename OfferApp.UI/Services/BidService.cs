using OfferApp.Shared.DTO;

namespace OfferApp.UI.Services
{
    public class BidService : IBidService
    {
        private readonly List<BidDto> _bids = new()
        {
            new BidDto { Id = 1, Name="Name#1", Created = DateTime.UtcNow, Description = "Description#1#2023", FirstPrice = 100 },
            new BidDto { Id = 2, Name="Name#2", Created = DateTime.UtcNow, Description = "Description#2#2023", FirstPrice = 200, Count = 2, LastPrice = 5000, Published = true, Updated = DateTime.UtcNow },
            new BidDto { Id = 3, Name="Name#3", Created = DateTime.UtcNow, Description = "Description#3#2023", FirstPrice = 300, Published = true },
        };

        public Task<BidDto> AddBid(BidDto dto)
        {
            dto.Id = _bids[^1].Id + 1;
            dto.Created = DateTime.UtcNow;
            _bids.Add(dto);
            return Task.FromResult(dto);
        }

        public Task<BidPublishedDto> BidUp(BidUpDto bidUp)
        {
            var bid = GetBid(bidUp.Id);
            if (!bid.Published)
            {
                throw new InvalidOperationException("Bid is not published");
            }

            var basePrice = bid.FirstPrice;
            if (bid.LastPrice is not null)
            {
                basePrice = bid.LastPrice.Value;
            }

            if (basePrice >= bidUp.Price)
            {
                throw new InvalidOperationException($"Bid price '{bidUp.Price}' cannot be less than '{basePrice}'");
            }

            bid.LastPrice = bidUp.Price;
            bid.Updated = DateTime.UtcNow;
            bid.Count++;
            return Task.FromResult(CreateBidPublished(bid));
        }

        public Task DeleteBid(int id)
        {
            var bid = GetBid(id);
            _bids.Remove(bid);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<BidDto>> GetAllBids()
        {
            await Task.CompletedTask;
            return _bids;
        }

        public async Task<IReadOnlyList<BidPublishedDto>> GetAllPublishedBids()
        {
            await Task.CompletedTask;
            return _bids.Where(b => b.Published)
                        .Select(CreateBidPublished)
                        .ToList();
        }

        public Task<BidDto?> GetBidById(int id)
        {
            return Task.FromResult(_bids.FirstOrDefault(b => b.Id == id));
        }

        public async Task<bool> Published(int id)
        {
            await Task.CompletedTask;
            var bid = GetBid(id);
            if (bid.Published)
            {
                return true;
            }

            bid.Published = true;
            return true;
        }

        public async Task<bool> Unpublished(int id)
        {
            await Task.CompletedTask;
            var bid = GetBid(id);
            if (!bid.Published)
            {
                return true;
            }

            bid.Published = false;
            bid.Updated = null;
            bid.LastPrice = null;
            bid.Count = 0;
            return true;
        }

        public Task<BidDto> UpdateBid(BidDto dto)
        {
            var bid = GetBid(dto.Id);
            bid.Name = dto.Name;
            bid.Description = dto.Description;
            bid.FirstPrice = dto.FirstPrice;
            return Task.FromResult(bid);
        }

        private BidDto GetBid(int id)
        {
            return _bids.FirstOrDefault(b => b.Id == id)
                ?? throw new InvalidOperationException("Bid not exists");
        }

        private BidPublishedDto CreateBidPublished(BidDto dto)
        {
            return new BidPublishedDto 
            { 
                Id = dto.Id,
                Name = dto.Name,
                Created = dto.Created,
                Description = dto.Description,
                FirstPrice = dto.FirstPrice,
                Count = dto.Count,
                LastPrice = dto.LastPrice,
                Updated = dto.Updated
            };
        }
    }
}
