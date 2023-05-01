using Auction_API.Infrastructure.Dto;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.BusinessUnit;

public interface IFavoriteBusinessUnit
{
    Task<Response> AddFavoriteAsync(FavoriteAddUpdateDto favoriteAddUpdateDto);
    Task<Favorite> GetMyFavoriteById(int favoriteId);
    Task<Response> DeleteFavoriteAsync(int favoriteId);
    Task<List<FavoriteDto>> ListMyAllFavorites(int userId);
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

    public async Task<Favorite> GetMyFavoriteById(int favoriteId)
    {
        var myFavorite = await _favoritesDataAccess.GetFavoritebyFavoriteId(favoriteId);
        return myFavorite;
    }

    public async Task<List<FavoriteDto>> ListMyAllFavorites(int userId)
    {
        var myFavorites = await _favoritesDataAccess.ListFavoriteByUserId(userId);
        return myFavorites;
    }
    public async Task<Response> DeleteFavoriteAsync(int favoriteId)
    {
        
        var favoriteEntity = await _favoritesDataAccess.GetFavoritebyFavoriteId(favoriteId);
        if (favoriteEntity == null)
            return new Response(ResponseCode.Success, "Böyle bir favori bulunmamaktadır.");

        var deleteEntity = await _favoritesDataAccess.DeleteFavoriteAsync(favoriteEntity);
        if (deleteEntity > 0)
            return new Response(ResponseCode.Success, "Favori Siliniyor...");

        return new Response(ResponseCode.Fail, "Favori silme işlemi başarısız.");

    }
    
}