namespace Auction_Project.Infrastructure.Entity;

public class Bids : BaseEntity
{
    public int AuctionId { get; set; }

    public int UserId { get; set; }

    public double  BidAmount { get; set; }

    public DateTime CreatedDate { get; set; }

}