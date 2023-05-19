using System;
using Auction_API.Infrastructure.Dto;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.BusinessUnit
{
    public interface IProductBusinessUnit
    {
        Task<List<Product>> GetUserProductsList(int userId);
    }

    public class ProductBusinessUnit : IProductBusinessUnit
	{

        private readonly IProductDataAccess _productDataAccess;

        public ProductBusinessUnit(IProductDataAccess productDataAccess)
        {
            _productDataAccess = productDataAccess;
        }

        public Task<List<Product>> GetUserProductsList(int userId)
        {
            return _productDataAccess.GetUserProductsList(userId);
        }
    }
}

