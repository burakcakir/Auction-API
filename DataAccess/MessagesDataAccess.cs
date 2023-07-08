using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.DataAccess;

public interface IMessagesDataAccess
{
    Task<int> Add(Messages message);
    Task<int> Update(Messages message);
    Task<int> Delete(Messages message);
}

public class MessagesDataAccess : IMessagesDataAccess
{
    private readonly AuctionContext _context;
    
    public MessagesDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public async Task<int> Add(Messages message)
    {
        _context.Messages.Add(message);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Update(Messages message)
    {
        _context.Messages.Update(message);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Delete(Messages message)
    {
        _context.Messages.Update(message);
        return await _context.SaveChangesAsync();
    }
}