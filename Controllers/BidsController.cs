using Auction_API.Infrastructure.Dto;
using Auction_Project.BusinessUnit;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Auction_API.Controllers;


[Route("bids/[controller]")]
[ApiController]
public class BidsController: ControllerBase
{
    private readonly IBidsBusinessUnit _bidsBusinessUnit;
    
    public BidsController(IBidsBusinessUnit bidsBusinessUnit)
    {
        _bidsBusinessUnit = bidsBusinessUnit;
    }
    
    [HttpPost]
    [Route("AddAsync")]
    public async Task<Response> AddAsync(BidsAddUpdateDto bidsAddUpdateDto)
    {
        return await _bidsBusinessUnit.AddAsync(bidsAddUpdateDto);
    }

    [HttpGet]
    [Route("GetMyLastBid")]
    public async Task<Bids> GetMyLastBid(int auctionId)
    {
        return await _bidsBusinessUnit.GetMyLastBid(auctionId);
    }
    
    [HttpGet]
    [Route("MyPastBids")]
    public async Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId)
    {
        return await _bidsBusinessUnit.MyPastBids(auctionId);
    }

    [HttpDelete]
    [Route("DeleteBids")]
    public async Task<Response> DeleteBids(int auctionId)
    {
        return await _bidsBusinessUnit.DeleteBidsAsync(auctionId);
    }
}