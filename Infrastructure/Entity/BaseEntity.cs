using System.Linq.Expressions;

namespace Auction_Project.Infrastructure.Entity;

public class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }


}