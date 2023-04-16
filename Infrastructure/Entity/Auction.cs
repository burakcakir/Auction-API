namespace Auction_Project.Infrastructure.Entity;

public class Auction : BaseEntity
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public double BuyNowPrice { get; set; }

    public double StartingPrice { get; set; }
    
    public double EndingPrice { get; set; }
    
    public double MinBidAmour { get; set; }
    
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int SellerId { get; set; }

    public int BuyerId { get; set; }

    public int ProductId { get; set; }
    
}