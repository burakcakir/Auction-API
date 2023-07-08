using System;
using Auction_API.Infrastructure;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.DataAccess
{
    public interface ISocketDataAccess
    {
        Task SendMessage(string userId,string message);
        Task SendMessageGroup(string groupName, string message);
        Task SendMessageToAll(string message);
        Task CreateGroup(string groupName);
        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);
    }

    public class SocketDataAccess : ISocketDataAccess
    {
        private readonly AuctionContext _context;
        private readonly SignalRHub _SignalRContext;


        public SocketDataAccess(AuctionContext context, SignalRHub SignalRContext)
        {
            _context = context;
            _SignalRContext = SignalRContext;
        }

        public async Task SendMessage(string userId, string message)
        {
            await _SignalRContext.SendMessagePrivate(userId, message);
        }

        public async Task SendMessageGroup(string groupName, string message)
        {
            await _SignalRContext.SendMessageToGroup(groupName, message);
        }

        public async Task SendMessageToAll(string message)
        {
            await _SignalRContext.SendMessageToAll(message);
        }

        public async Task CreateGroup(string groupName)
        {
            await _SignalRContext.CreateGroup(groupName);
        }

        public async Task AddToGroup(string groupName)
        {
            await _SignalRContext.AddToGroup(groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await _SignalRContext.RemoveFromGroup(groupName);
        }
    }
}

