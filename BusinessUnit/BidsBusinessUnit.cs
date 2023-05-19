using Auction_API.Infrastructure.Dto;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Auction_Project.BusinessUnit;

public interface IBidsBusinessUnit
{
    Task<Response> AddAsync(BidsAddUpdateDto bidsAddUpdateDto);
    Task<Bids> GetMyLastBid(int auctionId, int userId);
    Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId, int userId);
    Task<Response> DeleteBidsAsync(int auctionId, int userId);
}

public class BidsBusinessUnit : IBidsBusinessUnit
{
    private readonly IBidsDataAccess _bidsDataAccess;
    
    public BidsBusinessUnit(IBidsDataAccess bidsDataAccess)
    {
        _bidsDataAccess = bidsDataAccess;
    }
    
    public async Task<Response> AddAsync(BidsAddUpdateDto bidsAddUpdateDto)
    {
        var newEntity = new Bids
        {
           AuctionId = bidsAddUpdateDto.AuctionId,
           UserId = bidsAddUpdateDto.UserId,
           BidAmount = bidsAddUpdateDto.BidAmount,
           CreatedDate = bidsAddUpdateDto.CreatedDate
        }; 
            
        await _bidsDataAccess.AddAsync(newEntity);
        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Bids> GetMyLastBid(int auctionId, int userId)
    {
        var getlastbid = await _bidsDataAccess.GetMyLastBids(auctionId,userId);
        return getlastbid;
    }
    
    public async Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId, int userId)
    {
        
        var getlastbids = await _bidsDataAccess.MyPastBids(auctionId,userId);
        return getlastbids;
    }

    public async Task<Response> DeleteBidsAsync(int auctionId, int userId)
    {
        var getmyLastBid =await _bidsDataAccess.GetMyLastBids(auctionId, userId);
        var bidsId = getmyLastBid.Id;
        if (getmyLastBid == null)
        {
            return new Response(ResponseCode.Fail, "GetMyLastBids Entity not found.");
        }
        var deletechanges = await _bidsDataAccess.DeleteAsync(getmyLastBid);
        if (deletechanges > 0)
        {
            return new Response(ResponseCode.Success, "Bids Entity deleted.");
        }

        return new Response(ResponseCode.Fail, "Bids Entity not deleted.");

    }
    
}