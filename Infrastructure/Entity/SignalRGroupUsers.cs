using System;
namespace Auction_API.Infrastructure.Entity
{
	public class SignalRGroupUsers
	{
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

