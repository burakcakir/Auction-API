namespace Auction_Project.Infrastructure.Entity;

public class Order: BaseEntity
{
    public int AuctionId { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public double Price { get; set; }

    public DateTime CreatedTime { get; set; }

    public string DeliveryAddress { get; set; }
}