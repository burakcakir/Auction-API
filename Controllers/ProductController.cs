using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction_API.BusinessUnit;
using Auction_API.Infrastructure.Dto;
using Auction_Project.BusinessUnit;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Auction_API.Controllers
{
    [Route("product/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductBusinessUnit _productBusinessUnit;

        public ProductController(IProductBusinessUnit productBusinessUnit)
        {
            _productBusinessUnit = productBusinessUnit;
        }

        [HttpPost]
        [Route("AddProductAsync")]
        public async Task<Response> AddProductAsync([FromBody] ProductAddDto ProductAddUpdateDto)
        {
            return await _productBusinessUnit.AddProductAsync(ProductAddUpdateDto);
        }


        [HttpGet]
        [Route("GetProductsByUser")]
        public async Task<Response<List<Product>>> GetProductsByUser()
        {
            var productList = await _productBusinessUnit.GetUserProductsList();

            if (productList.Count > 0)
            {
                return new Response<List<Product>>(ResponseCode.Success, productList);

            }
            else
            {
                return new Response<List<Product>>(ResponseCode.NoContent);

            }
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<Response<List<Product>>> GetAllProducts()
        {
            var productList = await _productBusinessUnit.GetAllProducts();

            if (productList.Count > 0)
            {
                return new Response<List<Product>>(ResponseCode.Success, productList);

            }
            else
            {
                return new Response<List<Product>>(ResponseCode.NoContent);

            }
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<Response> UpdateProduct([FromBody] ProductUpdateDto product)
        {
            return await _productBusinessUnit.UpdateProduct(product);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<Response> DeleteProduct(int productId)
        {
            return await _productBusinessUnit.DeleteProduct(productId);
        }


    }
}

