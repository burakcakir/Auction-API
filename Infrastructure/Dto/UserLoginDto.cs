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

    public class UserLoginRequestDto
    {
        public string UserEmail { get; set; }

        public string Password { get; set; }
    }

	public class ChangePasswordInput
	{
        public string UserEmail { get; set; }

        public string Password { get; set; }
    }
}

