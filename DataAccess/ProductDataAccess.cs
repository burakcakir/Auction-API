using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IProductDataAccess
{
    int Add(Product product);
    int Update(Product product);
    int Delete(Product product);
    Task<List<Product>> GetUserProductsList(int userId);
}

public class ProductDataAccess : IProductDataAccess
{
    private readonly AuctionContext _context;
    
    public ProductDataAccess(AuctionContext context)
    {
        _context = context;
    }
    
    public int Add(Product product)
    {
        _context.Products.Add(product);
        return _context.SaveChanges();
    }

    public int Update(Product product)
    {
        _context.Products.Update(product);
        return _context.SaveChanges();
    }

    public int Delete(Product product)
    {
        _context.Products.Remove(product);
        return _context.SaveChanges();
    }

    public async Task<List<Product>> GetUserProductsList(int userId)
    {
        var getFavorite = await _context.Products.Where(x => x.SellerId == userId).ToListAsync();
        return getFavorite;
    }
}