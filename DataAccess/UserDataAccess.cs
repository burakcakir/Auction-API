using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IUserDataAccess
{
    int Add(User user);
    Task<int> Delete(User user);
    Task<int> Update(User user);
    User GetUserByUsername(string username);
    User GetUserByUserId(int userId);
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

    public async Task<int> Delete(User user)
    {
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync();
    }

    public User GetUserByUsername(string username)
    {
        var userInfo = _context.Users.Where(x => x.UserName == username && x.IsDeleted == false).FirstOrDefault();
        return userInfo;
    }

    public User GetUserByUserId(int userId)
    {
        var userData = _context.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefault();
        return userData;
    }

    public async Task<int> Update(User user)
    {
        _context.Set<User>().Update(user);
        return await _context.SaveChangesAsync();
    }

}