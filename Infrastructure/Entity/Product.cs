namespace Auction_Project.Infrastructure.Entity;

public class Product : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int SellerId { get; set; }

    public DateTime InsertionDate { get; set; }
    
}