using Auction_API.BusinessUnit;
using Auction_API.Infrastructure.Dto;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Auction_Project.BusinessUnit;

public interface IBidsBusinessUnit
{
    Task<Response> AddAsync(BidsAddUpdateDto bidsAddUpdateDto);
    Task<Bids> GetMyLastBid(int auctionId);
    Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId);
    Task<Response> DeleteBidsAsync(int auctionId);
}

public class BidsBusinessUnit : IBidsBusinessUnit
{
    private readonly IBidsDataAccess _bidsDataAccess;
    private readonly IUserBusinessUnit _userBusinessUnit;
    private readonly IUserDataAccess _userDataAccess;
    private readonly ISocketBusinessUnit _socketBusinessUnit;
    
    public BidsBusinessUnit(IBidsDataAccess bidsDataAccess, IUserBusinessUnit userBusinessUnit, IUserDataAccess userDataAccess, ISocketBusinessUnit socketBusinessUnit)
    {
        _bidsDataAccess = bidsDataAccess;
        _userBusinessUnit = userBusinessUnit;
        _userDataAccess = userDataAccess;
        _socketBusinessUnit = socketBusinessUnit;
    }
    
    public async Task<Response> AddAsync(BidsAddUpdateDto bidsAddUpdateDto)
    {
        var identityUserId =await _userBusinessUnit.GetUserId();
        var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);
        var newEntity = new Bids
        {
           AuctionId = bidsAddUpdateDto.AuctionId,
           UserId = user.Id,
           BidAmount = bidsAddUpdateDto.BidAmount,
           CreatedDate = bidsAddUpdateDto.CreatedDate
        }; 
            
        await _bidsDataAccess.AddAsync(newEntity);

        //Bu bid'e teklif veren kullanıcılara mesaj gönderir.
        await _socketBusinessUnit.SendMessageToGroupAsync(bidsAddUpdateDto.AuctionId.ToString(), "Takipte olduğunuz mezata yeni bir teklif verildi.");

        //Bid veren kullanıcıyı socket grubuna ekle
        await _socketBusinessUnit.AddToGroup(bidsAddUpdateDto.AuctionId.ToString());

        return new Response(ResponseCode.Success, "Success");
    }

    public async Task<Bids> GetMyLastBid(int auctionId)
    {
        var identityUserId =await _userBusinessUnit.GetUserId();
        var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);
        var getlastbid = await _bidsDataAccess.GetMyLastBids(auctionId,user.Id);
        return getlastbid;
    }
    
    public async Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId)
    {
        var identityUserId =await _userBusinessUnit.GetUserId();
        var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);   
        var getlastbids = await _bidsDataAccess.MyPastBids(auctionId,user.Id);
        return getlastbids;
    }

    public async Task<Response> DeleteBidsAsync(int auctionId)
    {
        var identityUserId =await _userBusinessUnit.GetUserId();
        var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);
        var getmyLastBid =await _bidsDataAccess.GetMyLastBids(auctionId, user.Id);
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