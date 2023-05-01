using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IFavoritesDataAccess
{
    Task<int> AddFavoriteAsync(Favorite favorite);
    Task<int> DeleteFavoriteAsync(Favorite favorite);
    Task<Favorite> GetFavoritebyFavoriteId(int favoriteId);
    Task<List<FavoriteDto>> ListFavoriteByUserId(int userId);
    Task<Response<FavoriteDto>> GetMyFavoriteDetailsByFavoriteId(int favoriteId);
    
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

    public async Task<List<FavoriteDto>> ListFavoriteByUserId(int userId)
    {
        var result = await (
            from favorite in _context.Favorites
            join a in _context.Auction on favorite.AuctionId equals a.Id
            join u in _context.Users on favorite.UserId equals u.Id
            where favorite.UserId == userId
            select new
            {
                Favorite = favorite,
                Auction = a,
                User = u
            }
        ).ToListAsync().ConfigureAwait(false);

        return result.Select(x => new FavoriteDto
        {
            AuctionName = x.Auction.Name,
            Description = x.Auction.Description,
            BuyNowPrice = x.Auction.BuyNowPrice,
            StartingPrice = x.Auction.StartingPrice,
            EndingPrice = x.Auction.EndingPrice,
            MinBidAmour = x.Auction.MinBidAmour,
            StartDate = x.Auction.StartDate,
            EndDate = x.Auction.EndDate,
            ProductId = x.Auction.ProductId,
            UserName = x.User.UserName,
            Name = x.User.Name,
            Surname = x.User.Surname,
            PhoneNumber = x.User.PhoneNumber,
            UserId = x.Favorite.UserId,
            AuctionId = x.Favorite.AuctionId,
            InsertionDate = x.Favorite.InsertionDate
        }).OrderBy(x => x.InsertionDate).ToList();
    }
    
    
    public async Task<Response<FavoriteDto>> GetMyFavoriteDetailsByFavoriteId(int favoriteId)
    {
        var result = await (
            from favorite in _context.Favorites
            join a in _context.Auction on favorite.AuctionId equals a.Id
            join u in _context.Users on favorite.UserId equals u.Id
            where favorite.Id == favoriteId
            select new
            {
                Favorite = favorite,
                Auction = a,
                User = u
            }
        ).FirstOrDefaultAsync();

        if (result == null)
            return new Response<FavoriteDto>(ResponseCode.Fail, "result is null");
        
        var favDto = new FavoriteDto
        {
            AuctionName = result.Auction.Name,
            Description = result.Auction.Description,
            BuyNowPrice = result.Auction.BuyNowPrice,
            StartingPrice = result.Auction.StartingPrice,
            EndingPrice = result.Auction.EndingPrice,
            MinBidAmour = result.Auction.MinBidAmour,
            StartDate = result.Auction.StartDate,
            EndDate = result.Auction.EndDate,
            ProductId = result.Auction.ProductId,
            UserName = result.User.UserName,
            Name = result.User.Name,
            Surname = result.User.Surname,
            PhoneNumber = result.User.PhoneNumber,
            UserId = result.Favorite.UserId,
            AuctionId = result.Favorite.AuctionId,
            InsertionDate = result.Favorite.InsertionDate
        };
        return new Response<FavoriteDto>(ResponseCode.Success, favDto, "favorite detay dönüşü başarılı");
    }
    public async Task<int> AddFavoriteAsync(Favorite favorite)
    {
        await _context.Set<Favorite>().AddAsync(favorite);
        return _context.SaveChanges();
    }
    public async Task<Favorite> GetFavoritebyFavoriteId (int favoriteId)
    {
        var getFavorite = await _context.Favorites.Where(x => x.Id == favoriteId).SingleAsync();
        return getFavorite;
    }
    public Task<int> DeleteFavoriteAsync(Favorite favorite)
    {
         _context.Set<Favorite>().Remove(favorite);
        return Task.FromResult(_context.SaveChanges());
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