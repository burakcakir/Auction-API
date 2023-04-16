using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.DataAccess;

public interface IFavoritesDataAccess
{
    int Add(Favorite favorite);
    int Update(Favorite favorite);
    int Delete(Favorite favorite);
}

public class FavoritesDataAccess : IFavoritesDataAccess
{
    private readonly AuctionContext _context;

    public FavoritesDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public int Add(Favorite favorite)
    {
        _context.Favorites.Add(favorite);
        return _context.SaveChanges();
    }

    public int Update(Favorite favorite)
    {
        _context.Favorites.Update(favorite);
        return _context.SaveChanges();
    }

    public int Delete(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        return _context.SaveChanges();
    }
}