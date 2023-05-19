using System;
namespace Auction_API.Infrastructure.Dto
{
	public class ProductAddDto
	{
        public string Name { get; set; }

        public string Description { get; set; }

        public int SellerId { get; set; }
    }

    public class ProductUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SellerId { get; set; }
    }

}

