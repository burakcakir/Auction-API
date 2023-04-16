namespace Auction_Project.Infrastructure.Entity;

public class AuctionAddUpdateDto
{
    public int  Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }

    public double? BuyNowPrice { get; set; } = 0;

    public double? StartingPrice { get; set; } = 0;

    public double? EndingPrice { get; set; } = 0;
    
    public double? MinBidAmour { get; set; } =0;

    public DateTime StartDate { get; set; } 

    public DateTime EndDate { get; set; }
    

    public int? SellerId { get; set; } = 0;

    public int? BuyerId { get; set; } = 0;

    public int? ProductId { get; set; } = 0;
}