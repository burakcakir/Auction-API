namespace Auction_Project.Infrastructure.Dto;

public class FavoriteAddUpdateDto
{
    public int UserId { get; set; }
    
    public int AuctionId { get; set; }
    
    public DateTime InsertionDate { get; set; }
}