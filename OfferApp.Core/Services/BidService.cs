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

        public BidDto AddBid(BidDto dto)
        {
            var bid = Bid.Create(dto.Name, dto.Description, dto.FirstPrice);
            _repository.Add(bid);
            return bid.AsDto();
        }

        public void DeleteBid(int id)
        {
            var bid = GetBid(id);
            _repository.Delete(bid);
        }

        public IReadOnlyList<BidDto> GetAllBids()
        {
            var bids = _repository.GetAll();
            var dtos = new List<BidDto>();

            foreach (var bid in bids)
            {
                dtos.Add(bid.AsDto());
            }

            return dtos;
        }

        public IReadOnlyList<BidPublishedDto> GetAllPublishedBids()
        {
            var bids = _repository.GetAll();
            var dtos = new List<BidPublishedDto>();

            foreach (var bid in bids)
            {
                if (bid.Published)
                {
                    dtos.Add(bid.AsPublishedDto());
                }
            }

            return dtos;
        }

        public BidDto? GetBidById(int id)
        {
            return _repository.Get(id)?.AsDto();
        }

        public bool Published(int id)
        {
            var bid = GetBid(id);

            if (bid.Published)
            {
                return true;
            }

            bid.Publish();
            _repository.Update(bid);
            return bid.Published;
        }

        public bool Unpublished(int id)
        {
            var bid = GetBid(id);

            if (!bid.Published)
            {
                return true;
            }

            bid.Unpublish();
            _repository.Update(bid);
            return !bid.Published;
        }

        public BidDto UpdateBid(BidDto dto)
        {
            var bid = GetBid(dto.Id);
            bid.ChangeName(dto.Name);
            bid.ChangeDescription(dto.Description);
            bid.ChangeFirstPrice(dto.FirstPrice);
            _repository.Update(bid);
            return bid.AsDto();
        }

        public BidPublishedDto BidUp(int id, decimal price)
        {
            var bid = GetBid(id);
            bid.ChangePrice(price);
            _repository.Update(bid);
            return bid.AsPublishedDto();
        }

        private Bid GetBid(int id)
        {
            var bid = _repository.Get(id);
            return bid is null ? throw new OfferException($"Bid with id '{id}' was not found") : bid;
        }
    }
}
