using System;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.Infrastructure.Dto
{
	public class UserLoginDto
	{
		public User userInformation { get; set; }

		public string userAccesToken { get; set; }

		public Response response { get; set; }
	}
}

