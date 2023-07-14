using Auction_API.Infrastructure.Dto;
using Auction_Project.BusinessUnit;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Auction_API.Controllers;
[Route("favorite/[controller]")]
[ApiController]
public class FavoriteController: ControllerBase
{
    private readonly IFavoriteBusinessUnit _favoriteBusinessUnit;

    public FavoriteController(IFavoriteBusinessUnit favoriteBusinessUnit)
    {
        _favoriteBusinessUnit = favoriteBusinessUnit;
    }
    
    [HttpPost]
    [Route("AddFavoriteAsync")]
    public async Task<Response> AddFavoriteAsync([FromBody] FavoriteAddUpdateDto favoriteAddUpdateDto)
    {
        return await _favoriteBusinessUnit.AddFavoriteAsync(favoriteAddUpdateDto);
    }

    [HttpGet]
    [Route("GetMyFavorite")]
    public async Task<Response<Favorite>> GetMyFavorite(int favoriteId)
    {
        var favoriteEntity = await _favoriteBusinessUnit.GetMyFavoriteById(favoriteId);
        return new Response<Favorite>(ResponseCode.Success, favoriteEntity, "success");
    }
    
    [HttpGet]
    [Route("GetMyFavoriteDetails")]
    public async Task<Response<FavoriteDto>> GetMyFavoriteDetails(int favoriteId)
    {
        var favoriteEntity = await _favoriteBusinessUnit.GetMyFavoriteDetails(favoriteId);
        return favoriteEntity;
    }
    
    [HttpGet]
    [Route("ListMyAllFavorites")]
    public async Task<List<FavoriteDto>> ListMyAllFavorites()
    {
        var favoriteList = await _favoriteBusinessUnit.ListMyAllFavorites();
        return favoriteList;
    }

    [HttpDelete]
    [Route("DiscardMyFavorite")]
    public async Task<Response> DeleteFavoriteAsync(int favoriteId)
    {
        return await _favoriteBusinessUnit.DeleteFavoriteAsync(favoriteId);
    }
}
