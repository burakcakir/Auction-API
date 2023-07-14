using Auction_Project.BusinessUnit;
using Microsoft.AspNetCore.Mvc;
using Auction_Project.Infrastructure.Entity;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;

namespace Auction_Project.Controllers;

[Route("auction/[controller]")]
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly IAuctionBusinessUnit _auctionBusinessUnit;

    public AuctionController(IAuctionBusinessUnit auctionBusinessUnit)
    {
        _auctionBusinessUnit = auctionBusinessUnit;
    }

    [HttpPost]
    [Route("AddAsync")]
    public async Task<Response> AddAsync([FromBody] AuctionAddUpdateDto auctionAddUpdateDto)
    {
        return await _auctionBusinessUnit.AddAsync(auctionAddUpdateDto);
    }

    [HttpGet]
    [Route("GetMyAuction")]
    public async Task<Auction> GetmyAuction(int auctionId)
    {
        return await _auctionBusinessUnit.GetMyAuction(auctionId);
    }
    
    [HttpGet]
    [Route("ListMyAuction")]
    public async Task<IList<AuctionDto>> ListmyAuction()
    {
        return await _auctionBusinessUnit.ListMyAuction();
    }
    
    [HttpPut]
    [Route("UpdateAuction")]
    public async Task<Response> UpdateAuction([FromBody] AuctionAddUpdateDto auctionAddUpdateDto)
    {
        return await _auctionBusinessUnit.UpdateAuction(auctionAddUpdateDto);
    }
    
    [HttpDelete]
    [Route("DeleteAuction")]
    public async Task<Response> DeleteAuction(int auctionId)
    {
        return await _auctionBusinessUnit.DeleteAuction(auctionId);
    }
    
    [HttpGet]
    [Route("ListAllAuction")]
    public async Task<IList<AuctionDto>> ListAllAuction()
    {
        return await _auctionBusinessUnit.GetAuctionByAllUsers();
    }
}
