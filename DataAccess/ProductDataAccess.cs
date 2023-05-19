using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_Project.DataAccess;

public interface IProductDataAccess
{
    int Add(Product product);
    int Update(Product product);
    Task<int> Delete(Product product);
    Task<Product> GetProductByProductId(int productId);
    Task<List<Product>> GetUserProductsList(int userId);
    Task<List<Product>> GetAllProducts();
    Task<int> AddProductAsync(Product product);
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

    public async Task<int> Delete(Product product)
    {
        _context.Products.Remove(product);
        return await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetUserProductsList(int userId)
    {
        var getProducts = await _context.Products.Where(x => x.SellerId == userId).ToListAsync();
        return getProducts;
    }

    public async Task<Product> GetProductByProductId(int productId)
    {
        var getProduct = await _context.Products.Where(x => x.Id == productId).SingleAsync();
        return getProduct;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var productList = await _context.Products.Where(x => x.IsDeleted == false).ToListAsync();
        return productList;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        await _context.Set<Product>().AddAsync(product);
        return _context.SaveChanges();
    }
}