using System;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.Infrastructure.Dto
{
	public class GetUserOutput
	{
        public GetUserEntity? userInformation { get; set; }

        public Response response { get; set; }
    }
}

