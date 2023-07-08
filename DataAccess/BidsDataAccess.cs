using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IBidsDataAccess
{
    int Add(Bids bids);
    int Update(Bids bids);
    int Delete(Bids bids);
    Task<int> AddAsync(Bids bids);
    Task<Bids> GetMyLastBids(int auctionId, int userId);
    Task<List<BidsAddUpdateDto>> MyPastBids(int auctionId, int userId);
    Task<int> DeleteAsync(Bids bids);
}

public class BidsDataAccess : IBidsDataAccess
{
    private readonly AuctionContext _context;

    public BidsDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Bids bids)
    {
        await _context.Set<Bids>().AddAsync(bids);
        return await _context.SaveChangesAsync();
    }
    
    public async Task<Bids> GetMyLastBids (int auctionId, int userId)
    {
        var result = await
            (from Bids in _context.Bids
                where Bids.AuctionId == auctionId && Bids.UserId == userId
                select new Bids
                {
                    AuctionId = Bids.AuctionId,
                    UserId = Bids.UserId,
                    BidAmount = Bids.BidAmount,
                    CreatedDate = Bids.CreatedDate,
                    Id =Bids.Id
                }).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        return result;
    }

    public async Task<List<BidsAddUpdateDto>> MyPastBids (int auctionId, int userId)
    {
        var result = await
            (from Bids in _context.Bids
                where Bids.AuctionId == auctionId && Bids.UserId == userId
                select new BidsAddUpdateDto
                {
                    AuctionId = Bids.AuctionId,
                    UserId = Bids.UserId,
                    BidAmount = Bids.BidAmount,
                    CreatedDate = Bids.CreatedDate
                }).OrderByDescending(x => x.CreatedDate).ToListAsync();
        return result;
    }
    
    public async Task<int> DeleteAsync(Bids bids)
    {
        _context.Set<Bids>().Remove(bids);
        return _context.SaveChanges();
    }

    public int Add(Bids bids)
    {
        _context.Bids.Add(bids);
        return _context.SaveChanges();
    }

    public int Update(Bids bids)
    {
        _context.Bids.Update(bids);
        return _context.SaveChanges();
    }

    public int Delete(Bids bids)
    {
        _context.Bids.Remove(bids);
        return _context.SaveChanges();
    }
    
}