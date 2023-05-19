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

        [HttpGet]
        [Route("GetProductsByUser")]
        public async Task<Response<List<Product>>> GetProductsByUser(int userId)
        {
            var favoriteEntity = await _productBusinessUnit.GetUserProductsList(userId);

            if (favoriteEntity.Count > 0)
            {
                return new Response<List<Product>>(ResponseCode.Success, favoriteEntity);

            }
            else
            {
                return new Response<List<Product>>(ResponseCode.NoContent, favoriteEntity);

            }
        }



    }
}

