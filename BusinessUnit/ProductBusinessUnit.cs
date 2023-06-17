using System;
using Auction_API.Infrastructure.Dto;
using Auction_Project.BusinessUnit;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.BusinessUnit
{
    public interface IProductBusinessUnit
    {
        Task<List<Product>> GetUserProductsList();
        Task<Response> UpdateProduct(ProductUpdateDto product);
        Task<Response> DeleteProduct(int productId);
        Task<List<Product>> GetAllProducts();
        Task<Response> AddProductAsync(ProductAddDto ProductAddUpdateDto);
    }

    public class ProductBusinessUnit : IProductBusinessUnit
	{

        private readonly IProductDataAccess _productDataAccess;
        private readonly IUserDataAccess _userDataAccess;
        private readonly IUserBusinessUnit _userBusinessUnit;

        public ProductBusinessUnit(IProductDataAccess productDataAccess, IUserDataAccess userDataAccess, IUserBusinessUnit userBusinessUnit)
        {
            _productDataAccess = productDataAccess;
            _userBusinessUnit = userBusinessUnit;
            _userDataAccess = userDataAccess;
        }

        public async Task<Response> AddProductAsync(ProductAddDto ProductAddUpdateDto)
        {
            var newEntity = new Product
            {
                SellerId = ProductAddUpdateDto.SellerId,
                Name = ProductAddUpdateDto.Name,
                Description = ProductAddUpdateDto.Description,
                InsertionDate = DateTime.UtcNow

            };
            await _productDataAccess.AddProductAsync(newEntity);
            return new Response(ResponseCode.Success);
        }

        public async Task<Response> DeleteProduct(int productId)
        {
            var productEntity = await _productDataAccess.GetProductByProductId(productId);
            if (productEntity == null)
                return new Response(ResponseCode.Success, "Böyle bir ürün bulunmamaktadır.");

            var deleteEntity = await _productDataAccess.Delete(productEntity);
            if (deleteEntity > 0)
                return new Response(ResponseCode.Success, "Ürün silme işlemi başarılı.");

            return new Response(ResponseCode.Fail, "Ürün silme işlemi başarısız.");
        }

        public Task<List<Product>> GetAllProducts()
        {
            return _productDataAccess.GetAllProducts();
        }

        public async Task<List<Product>> GetUserProductsList()
        {
            var identityUserId =await _userBusinessUnit.GetUserId();
            var user = await _userDataAccess.GetUserByIdentityUserId(identityUserId);
            return await _productDataAccess.GetUserProductsList(user.Id);
        }

        public async Task<Response> UpdateProduct(ProductUpdateDto product)
        {
            var productEntity = await _productDataAccess.GetProductByProductId(product.Id);

            productEntity.Name = product.Name;
            productEntity.SellerId = product.SellerId;
            productEntity.Description = product.Description;
            
            var result = _productDataAccess.Update(productEntity);

            if(result > 0)
            {
                return new Response(ResponseCode.Success,"Güncelleme başarılı");
            }
            else
            {
                return new Response(ResponseCode.Fail, "Güncelleme başarısız");
            }
        }

       
    }
}

