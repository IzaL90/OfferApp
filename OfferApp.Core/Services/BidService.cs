using OfferApp.Core.DTO;
using OfferApp.Core.Exceptions;
using OfferApp.Domain.Entities;
using OfferApp.Domain.Exceptions;
using OfferApp.Core.Mappings;
using OfferApp.Domain.Repositories;

namespace OfferApp.Core.Services
{
    internal sealed class BidService : IBidService
    {
        private readonly IBidRepository _repository;

        public BidService(IBidRepository repository)
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
            return (await _repository.GetAll())
                .Select(bid => bid.AsDto())
                .ToList();
        }

        public async Task<IReadOnlyList<BidPublishedDto>> GetAllPublishedBids()
        {
            return (await _repository.GetAllPublished())
                        .Select(b => b.AsPublishedDto())
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
            if (bid.Published)
            {
                throw new OfferException($"Bid with id '{dto.Id}' is published and cannot be updated");
            }

            bid.ChangeName(dto.Name);
            bid.ChangeDescription(dto.Description);
            bid.ChangeFirstPrice(dto.FirstPrice);
            await _repository.Update(bid);
            return bid.AsDto();
        }

        public async Task<BidPublishedDto> BidUp(BidUpDto bidUp)
        {
            var bid = await GetBid(bidUp.Id);
            bid.ChangePrice(bidUp.Price);
            await _repository.Update(bid);
            return bid.AsPublishedDto();
        }

        private async Task<Bid> GetBid(int id)
        {
            var bid = await _repository.Get(id);
            return bid is null ? throw new ResourceNotFoundException($"Bid with id '{id}' was not found") : bid;
        }
    }
}
