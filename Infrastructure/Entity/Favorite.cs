namespace Auction_Project.Infrastructure.Entity;

public class Favorite : BaseEntity
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }

    public DateTime InsertionDate { get; set; }
    
}