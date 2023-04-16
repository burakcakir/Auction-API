using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.DataAccess;

public interface IUserDataAccess
{
    int Add(User user);
    int Update(User user);
    int Delete(User user);
}

public class UserDataAccess : IUserDataAccess
{
    private readonly AuctionContext _context;

    public UserDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public int Add(User user)
    {
        _context.Users.Add(user);
        return _context.SaveChanges();
    }

    public int Update(User user)
    {
        _context.Users.Update(user);
        return _context.SaveChanges();
    }

    public int Delete(User user)
    {
        _context.Users.Remove(user);
        return _context.SaveChanges();
    }
}