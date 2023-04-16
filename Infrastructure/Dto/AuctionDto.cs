using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.Infrastructure.Dto;

public class AuctionDto
{
    public string AuctionName { get; set; }
    
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

    
    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; } 
    
    public string PhoneNumber { get; set; }
}