using Auction_API.BusinessUnit;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.BusinessUnit
{
    public interface IAuctionBusinessUnit
    {
        Task<Response> AddAsync(AuctionAddUpdateDto auctionAddUpdateDto);
        Task<Auction> GetMyAuction(int auctionId);
        Task<IList<AuctionDto>> ListMyAuction();
        Task<Response> UpdateAuction(AuctionAddUpdateDto auctionAddUpdateDto);
        Task<Response> DeleteAuction(int auctionId);
        Task<List<AuctionDto>> GetAuctionByAllUsers();
    }

    public class AuctionBusinessUnit : IAuctionBusinessUnit
    {
        private readonly IAuctionDataAccess _auctionDataAccess;
        private readonly IUserBusinessUnit _userBusinessUnit;
        private readonly IUserDataAccess _userDataAccess;
        private readonly ISocketBusinessUnit _socketBusinessUnit;

        public AuctionBusinessUnit(IAuctionDataAccess auctionDataAccess,IUserBusinessUnit userBusinessUnit,IUserDataAccess userDataAccess, ISocketBusinessUnit socketBusinessUnit)
        {
            _auctionDataAccess = auctionDataAccess;
            _userBusinessUnit = userBusinessUnit;
            _userDataAccess = userDataAccess;
            _socketBusinessUnit = socketBusinessUnit;
        }

        public async Task<Response> AddAsync(AuctionAddUpdateDto auctionAddUpdateDto)
        {
            var newEntity = new Auction
            {
                Name = auctionAddUpdateDto.Name,
                Description = auctionAddUpdateDto.Description,
                BuyNowPrice = auctionAddUpdateDto.BuyNowPrice ?? 0,
                StartingPrice = auctionAddUpdateDto.StartingPrice ?? 0,
                EndingPrice = auctionAddUpdateDto.EndingPrice ?? 0,
                MinBidAmour = auctionAddUpdateDto.MinBidAmour?? 0,
                StartDate = auctionAddUpdateDto.StartDate ,
                EndDate = auctionAddUpdateDto.EndDate,
                SellerId = auctionAddUpdateDto.SellerId ?? 0,
                BuyerId = auctionAddUpdateDto.BuyerId ?? 0,
                ProductId = auctionAddUpdateDto.ProductId ?? 0
            }; 
            
            await _auctionDataAccess.AddAsync(newEntity);

            //oluşturulan auction için socket'te grup oluştur
            int auctionId = await _auctionDataAccess.GetAuctionbyAuctionNameAndSellerId(newEntity.Name,newEntity.SellerId);

            await _socketBusinessUnit.CreateGroup(auctionId.ToString());

            return new Response(ResponseCode.Success, "Success");
        } 

        public async Task<Auction> GetMyAuction(int auctionId)
        {
            var myAuction = await _auctionDataAccess.GetAuctionbyAuctionId(auctionId);
            return myAuction;
        }

        public async Task<IList<AuctionDto>> ListMyAuction()
        {
            var identityUserId =await _userBusinessUnit.GetUserId();
            var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);
            var myList = await _auctionDataAccess.ListAuctionbySellerId(user.Id);
            return myList;
        }
        
        public async Task<Response> UpdateAuction(AuctionAddUpdateDto auctionAddUpdateDto)
        {

            var auctionEntity = await _auctionDataAccess.GetAuctionbyAuctionId(auctionAddUpdateDto.Id);
            auctionEntity.Name = auctionAddUpdateDto.Name;
            auctionEntity.Description = auctionAddUpdateDto.Description;
            auctionEntity.BuyNowPrice = auctionAddUpdateDto.BuyNowPrice ??0;
            auctionEntity.StartingPrice = auctionAddUpdateDto.StartingPrice??0;
            auctionEntity.EndingPrice = auctionAddUpdateDto.EndingPrice??0;
            auctionEntity.MinBidAmour = auctionAddUpdateDto.MinBidAmour??0;
            auctionEntity.StartDate = auctionAddUpdateDto.StartDate ;
            auctionEntity.EndDate = auctionAddUpdateDto.EndDate;
            auctionEntity.SellerId = auctionAddUpdateDto.SellerId??0;
            auctionEntity.BuyerId = auctionAddUpdateDto.BuyerId??0;
            auctionEntity.ProductId = auctionAddUpdateDto.ProductId??0;
            

            var saveChangesValue = await _auctionDataAccess.UpdateAsync(auctionEntity);
            if(saveChangesValue>0)
                return new Response(ResponseCode.Success, "deneme");

            return new Response(ResponseCode.Fail, "Fail");
        }

        public async Task<Response> DeleteAuction(int auctionId)
        {
            var auctionEntity = await _auctionDataAccess.GetAuctionbyAuctionId(auctionId);
            if (auctionEntity == null)
            {
                return new Response(ResponseCode.Fail, "Auction Entity is null");
            }

            var deleteChanges = await _auctionDataAccess.DeleteAsync(auctionEntity);
            if (deleteChanges > 0)
            {
                return new Response(ResponseCode.Success, "success");
            }
            return new Response(ResponseCode.Fail, "Silme işlemi başarısız");
        }

        public async Task<List<AuctionDto>> GetAuctionByAllUsers()
        {
            var auction = await _auctionDataAccess.ListAuctionByAllUser();
            return auction.ToList();
        }
    }
}