using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IUserDataAccess
{
    int Add(User user);
    Task<int> Delete(User user);
    Task<int> Update(User user);
    User GetUserByUsername(string username);
    User GetUserByUserId(int userId);
    User GetUserByEmail(string eMail);
    Task<User> GetUserByIdentityUserId(string identityUserId);
}

public class UserDataAccess : IUserDataAccess
{
    private readonly AuctionContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly UserManager<IdentityUser> _userManager;

    public UserDataAccess(AuctionContext context, IHttpContextAccessor httpContext,UserManager<IdentityUser> userManager)
    {
        _context = context;
        _httpContext = httpContext;
        _userManager = userManager;
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

    public async Task<User> GetUserByIdentityUserId(string identityUserId)
    {
        var userEmail =await _userManager.Users.Where(x => x.Id == identityUserId).Select(x => x.Email).AsNoTracking()
            .SingleAsync();
        var query =await _context.Users.Where(y => y.Email.Equals(userEmail.ToLower())).AsNoTracking().SingleAsync();
        return query;
    }

    public User GetUserByUserId(int userId)
    {
        var userData = _context.Users.Where(x => x.Id == userId && x.IsDeleted == false).FirstOrDefault();
        return userData;
    }

    public User GetUserByEmail(string eMail)
    {
        var userData = _context.Users.Where(x => x.Email == eMail && x.IsDeleted == false).FirstOrDefault();
        return userData;
    }

    public async Task<int> Update(User user)
    {
        _context.Set<User>().Update(user);
        return await _context.SaveChangesAsync();
    }

}