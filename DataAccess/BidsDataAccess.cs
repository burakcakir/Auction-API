using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.DataAccess;

public interface IBidsDataAccess
{
    int Add(Bids bids);
    int Update(Bids bids);
    int Delete(Bids bids);
}

public class BidsDataAccess : IBidsDataAccess
{
    private readonly AuctionContext _context;

    public BidsDataAccess(AuctionContext context)
    {
        _context = context;
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