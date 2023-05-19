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
    public async Task<Bids> GetMyLastBid(int auctionId, int userId)
    {
        return await _bidsBusinessUnit.GetMyLastBid(auctionId,userId);
    }
    
    [HttpGet]
    [Route("MyPastBids")]
    public async Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId, int userId)
    {
        return await _bidsBusinessUnit.MyPastBids(auctionId,userId);
    }

    [HttpDelete]
    [Route("DeleteBids")]
    public async Task<Response> DeleteBids(int auctionId, int userId)
    {
        return await _bidsBusinessUnit.DeleteBidsAsync(auctionId, userId);
    }
}