using System;
namespace Auction_API.Infrastructure.Dto
{
	public class GetUserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}

