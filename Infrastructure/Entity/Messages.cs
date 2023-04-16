namespace Auction_Project.Infrastructure.Entity;

public class Messages : BaseEntity
{
    public int SenderId { get; set; }

    public int RecipientId { get; set; }

    public string Content { get; set; }

    public DateTime InsertionDate { get; set; }
}