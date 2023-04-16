using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.DataAccess;

public interface IMessagesDataAccess
{
    int Add(Messages message);
    int Update(Messages message);
    int Delete(Messages message);
}

public class MessagesDataAccess : IMessagesDataAccess
{
    private readonly AuctionContext _context;
    
    public MessagesDataAccess(AuctionContext context)
    {
        _context = context;
    }
    
    public int Add(Messages message)
    {
        _context.Messages.Add(message);
        return _context.SaveChanges();
    }

    public int Update(Messages message)
    {
        _context.Messages.Update(message);
        return _context.SaveChanges();
    }

    public int Delete(Messages message)
    {
        _context.Messages.Update(message);
        return _context.SaveChanges();
    }
}