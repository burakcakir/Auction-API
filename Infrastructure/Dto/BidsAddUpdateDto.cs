namespace Auction_API.Infrastructure.Dto;

public class BidsAddUpdateDto
{
    public int AuctionId { get; set; }
    

    public double  BidAmount { get; set; }

    public DateTime CreatedDate { get; set; }
}