using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IOrderDataAccess
{
    int Add(Order order);
    int Update(Order order);
    int Delete(Order order);
    Task<List<Order>> ListMyAllOrder(int userid);
}

public class OrderDataAccess : IOrderDataAccess
{
    private readonly AuctionContext _context;

    public OrderDataAccess(AuctionContext context)
    {
        _context = context;
    }

    public int Add(Order order)
    {
        _context.Orders.Add(order);
        return _context.SaveChanges();
    }

    public int Update(Order order)
    {
        _context.Orders.Update(order);
        return _context.SaveChanges();
    }

    public int Delete(Order order)
    {
        _context.Orders.Remove(order);
        return _context.SaveChanges();
    }

    public async Task<List<Order>> ListMyAllOrder(int userid)
    {
        var query = await _context.Orders.Where(x => x.BuyerId == userid).ToListAsync();
        return query;
    }
}