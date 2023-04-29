using Auction_Project.BusinessUnit;
using Auction_Project.Controllers;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
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
    public async Task<Response> AddFavoriteAsync(FavoriteAddUpdateDto favoriteAddUpdateDto)
    {
        return await _favoriteBusinessUnit.AddFavoriteAsync(favoriteAddUpdateDto);
    }
}
