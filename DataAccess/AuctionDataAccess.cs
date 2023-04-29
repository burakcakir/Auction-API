using System.Data;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Auction_Project.Infrastructure.Dto;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IAuctionDataAccess
{
    Task<int> AddAsync(Auction auction);
    Task<Auction> GetAuctionbyAuctionId(int auctionId);
    Task<IList<AuctionDto>> ListAuctionbySellerId (int sellerId);
    int Update(Auction auction);
    int Delete(Auction auction);
    Task<int> UpdateAsync(Auction auction);
    Task<int> DeleteAsync(Auction auction);
    Task<IList<AuctionDto>> ListAuctionByAllUser ();
}

public class AuctionDataAccess : IAuctionDataAccess
{
    private readonly AuctionContext _context;

    public AuctionDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Auction auction)
    {
        await _context.Set<Auction>().AddAsync(auction);
        return await _context.SaveChangesAsync();
    }

    public async Task<Auction> GetAuctionbyAuctionId (int auctionId)
    {
        var getAuction = await _context.Auction.Where(x => x.Id == auctionId).SingleAsync();
        return getAuction;
    }
    
    public async Task<IList<AuctionDto>> ListAuctionbySellerId (int sellerId)
    {
        var result = await
            (from auction in _context.Auction
                join usr in _context.Users on auction.SellerId equals usr.Id into user
                from usr in user.DefaultIfEmpty<User?>()
                where auction.SellerId == sellerId
                select new AuctionDto
                {
                    AuctionName = auction.Name,
                    Description = auction.Description,
                    BuyNowPrice = auction.BuyNowPrice,
                    StartingPrice = auction.StartingPrice,
                    EndingPrice = auction.EndingPrice,
                    MinBidAmour = auction.MinBidAmour,
                    StartDate = auction.StartDate,
                    EndDate = auction.EndDate,
                    SellerId = usr.Id,
                    BuyerId = auction.BuyerId,
                    ProductId = auction.ProductId,
                    UserName = usr.UserName,
                    Name = usr.Name,
                    Surname = usr.Surname,
                    PhoneNumber = usr.PhoneNumber
                }).OrderBy(x => x.StartDate).ToListAsync();
        return result;
    }

    public async Task<IList<AuctionDto>> ListAuctionByAllUser ()
    {
        var result = await
            (from auction in _context.Auction
                join usr in _context.Users on auction.SellerId equals usr.Id into user
                from usr in user.DefaultIfEmpty<User?>()
                select new AuctionDto
                {
                    AuctionName = auction.Name,
                    Description = auction.Description,
                    BuyNowPrice = auction.BuyNowPrice,
                    StartingPrice = auction.StartingPrice,
                    EndingPrice = auction.EndingPrice,
                    MinBidAmour = auction.MinBidAmour,
                    StartDate = auction.StartDate,
                    EndDate = auction.EndDate,
                    SellerId = usr.Id,
                    BuyerId = auction.BuyerId,
                    ProductId = auction.ProductId,
                    UserName = usr.UserName,
                    Name = usr.Name,
                    Surname = usr.Surname,
                    PhoneNumber = usr.PhoneNumber
                }).OrderBy(x => x.StartDate).ToListAsync();
        return result;
    }
    public int Update(Auction auction)
    {
        _context.Auction.Update(auction);
        return _context.SaveChanges();
    }

    public int Delete(Auction auction)
    {
        _context.Auction.Remove(auction);
        return _context.SaveChanges();
    }
    public async Task<int> UpdateAsync (Auction auction)
    {
        _context.Set<Auction>().Update(auction);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Auction auction)
    {
        _context.Set<Auction>().Remove(auction);
        return await _context.SaveChangesAsync();
    }
}