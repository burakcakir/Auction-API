using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.BusinessUnit;

public interface IFavoriteBusinessUnit
{
    Task<Response> AddFavoriteAsync(FavoriteAddUpdateDto favoriteAddUpdateDto);
}

public class FavoriteBusinessUnit : IFavoriteBusinessUnit
{
    private readonly IFavoritesDataAccess _favoritesDataAccess;
    private readonly IAuctionDataAccess _auctionDataAccess;

    public FavoriteBusinessUnit(IFavoritesDataAccess favoritesDataAccess, IAuctionDataAccess auctionDataAccess)
    {
        _favoritesDataAccess = favoritesDataAccess;
        _auctionDataAccess = auctionDataAccess;
    }

    public async Task<Response> AddFavoriteAsync(FavoriteAddUpdateDto favoriteAddUpdateDto)
    {
        var newEntity = new Favorite
        {
            UserId = favoriteAddUpdateDto.UserId,
            AuctionId = favoriteAddUpdateDto.UserId,
            InsertionDate = DateTime.UtcNow
            
        };
        await _favoritesDataAccess.AddFavoriteAsync(newEntity);
        return new Response(ResponseCode.Success, "success");
    }
    
}