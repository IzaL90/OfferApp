using OfferApp.Core.DTO;
using OfferApp.Core.Entities;
using OfferApp.Core.Exceptions;
using OfferApp.Core.Mappings;
using OfferApp.Core.Repositories;

namespace OfferApp.Core.Services
{
    internal sealed class BidService : IBidService
    {
        private readonly IRepository<Bid> _repository;

        public BidService(IRepository<Bid> repository)
        {
            _repository = repository;
        }

        public async Task<BidDto> AddBid(BidDto dto)
        {
            var bid = Bid.Create(dto.Name, dto.Description, dto.FirstPrice);
            await _repository.Add(bid);
            return bid.AsDto();
        }

        public async Task DeleteBid(int id)
        {
            var bid = await GetBid(id);
            await _repository.Delete(bid);
        }

        public async Task<IReadOnlyList<BidDto>> GetAllBids()
        {
            await Task.CompletedTask;
            return (await _repository.GetAll())
                .Select(bid => bid.AsDto())
                .ToList();
        }

        public async Task<IReadOnlyList<BidPublishedDto>> GetAllPublishedBids()
        {
            await Task.CompletedTask;
            return (await _repository.GetAll())
                .Where(bid => bid.Published)
                .Select(bid => bid.AsPublishedDto())
                .ToList();
        }

        public async Task<BidDto?> GetBidById(int id)
        {
            return (await _repository.Get(id))?.AsDto();
        }

        public async Task<bool> Published(int id)
        {
            var bid = await GetBid(id);

            if (bid.Published)
            {
                return true;
            }

            bid.Publish();
            await _repository.Update(bid);
            return bid.Published;
        }

        public async Task<bool> Unpublished(int id)
        {
            var bid = await GetBid(id);

            if (!bid.Published)
            {
                return true;
            }

            bid.Unpublish();
            await _repository.Update(bid);
            return !bid.Published;
        }

        public async Task<BidDto> UpdateBid(BidDto dto)
        {
            var bid = await GetBid(dto.Id);
            bid.ChangeName(dto.Name);
            bid.ChangeDescription(dto.Description);
            bid.ChangeFirstPrice(dto.FirstPrice);
            await _repository.Update(bid);
            return bid.AsDto();
        }

        public async Task<BidPublishedDto> BidUp(int id, decimal price)
        {
            var bid = await GetBid(id);
            bid.ChangePrice(price);
            await _repository.Update(bid);
            return bid.AsPublishedDto();
        }

        private async Task<Bid> GetBid(int id)
        {
            var bid = await _repository.Get(id);
            return bid is null ? throw new OfferException($"Bid with id '{id}' was not found") : bid;
        }
    }
}
