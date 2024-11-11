﻿using Microsoft.AspNetCore.Mvc;
using OfferApp.Shared.DTO;
using OfferApp.Core.Services;

namespace OfferApp.Api.Controllers
{
    public class BidsController : BaseController
    {
        private readonly IBidService _bidService;

        public BidsController(IBidService bidService)
        {
            _bidService = bidService;            
        }

        [HttpGet]
        public async Task<IEnumerable<BidDto>> GetAll()
        {
            return await _bidService.GetAllBids();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BidDto?>> Get(int id)
        {
            var bid = await _bidService.GetBidById(id);
            return OkOrNotFound(bid);
        }

        [HttpPost]
        public async Task<ActionResult> Add(BidDto bidDto)
        {
            var dto = await _bidService.AddBid(bidDto);
            return CreatedAtAction(nameof(Get), new { dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, BidDto bidDto)
        {
            bidDto.Id = id;
            await _bidService.UpdateBid(bidDto);
            return Ok(bidDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _bidService.DeleteBid(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/publish")]
        public async Task<ActionResult> Publish(int id)
        {
            var published = await _bidService.Published(id);
            return published ? NoContent() : BadRequest();
        }
        
        [HttpPatch("{id:int}/unpublish")]
        public async Task<ActionResult> Unpublish(int id)
        {
            var published = await _bidService.Unpublished(id);
            return published ? NoContent() : BadRequest();
        }

        [HttpPatch("{id:int}/bid-up")]
        public async Task<ActionResult> BidUp(int id, BidUpDto dto)
        {
            dto.Id = id;
            return Ok(await _bidService.BidUp(dto));
        }

        [HttpGet("published")]
        public async Task<IEnumerable<BidPublishedDto>> GetAllPublished()
        {
            return await _bidService.GetAllPublishedBids();
        }
    }
}
