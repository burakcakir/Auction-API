using System;
using Auction_API.DataAccess;
using Auction_API.Infrastructure;
using Auction_API.Infrastructure.Dto;
using Auction_Project.BusinessUnit;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Auction_Project.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auction_API.BusinessUnit
{
    public interface ISocketBusinessUnit
    {
        Task<Response>  SendMessageAsync(string userId,string message);
        Task<Response> SendMessageToAllAsync(string message);
        Task<Response> SendMessageToGroupAsync(string groupName, string message);
        Task CreateGroup(string groupName);
        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);
    }

    public class SocketBusinessUnit : ISocketBusinessUnit
	{
        private readonly ISocketDataAccess _socketDataAccess;
        private readonly IProductDataAccess _productDataAccess;
        private readonly IUserDataAccess _userDataAccess;
        private readonly IUserBusinessUnit _userBusinessUnit;
        private readonly IMessagesDataAccess _messagesDataAccess;

        public SocketBusinessUnit(IProductDataAccess productDataAccess, IUserDataAccess userDataAccess, IUserBusinessUnit userBusinessUnit, ISocketDataAccess socketDataAccess, IMessagesDataAccess messagesDataAccess)
        {
            _productDataAccess = productDataAccess;
            _userBusinessUnit = userBusinessUnit;
            _userDataAccess = userDataAccess;
            _socketDataAccess = socketDataAccess;
            _messagesDataAccess = messagesDataAccess;
        }

        public async Task<Response> SendMessageAsync(string userId, string message)
        {
            try
            {
                await _socketDataAccess.SendMessage(userId, message);

                Messages _message = new Messages();
                _message.Content = Utility.ComputeSHA512(message);
                _message.RecipientId = Convert.ToInt32(userId);
                _message.InsertionDate = DateTime.Now;

                await _messagesDataAccess.Add(_message);

                return new Response(ResponseCode.Success);
            }
            catch (Exception)
            {
                return new Response(ResponseCode.Fail);
            }
        }

        public async Task<Response> SendMessageToAllAsync(string message)
        {
            try
            {
                await _socketDataAccess.SendMessageToAll(message);

                Messages _message = new Messages();
                _message.Content = Utility.ComputeSHA512(message);
                _message.RecipientId = 0;
                _message.InsertionDate = DateTime.Now;

                await _messagesDataAccess.Add(_message);

                return new Response(ResponseCode.Success);
            }
            catch (Exception)
            {
                return new Response(ResponseCode.Fail);
            }
        }

        public async Task<Response> SendMessageToGroupAsync(string groupName, string message)
        {
            try
            {
                await _socketDataAccess.SendMessageGroup(groupName,message);

                Messages _message = new Messages();
                _message.Content = Utility.ComputeSHA512(message);
                _message.RecipientId = 0;
                _message.InsertionDate = DateTime.Now;

                await _messagesDataAccess.Add(_message);

                return new Response(ResponseCode.Success);
            }
            catch (Exception)
            {
                return new Response(ResponseCode.Fail);
            }
        }

        public async Task CreateGroup(string groupName)
        {
            await _socketDataAccess.CreateGroup(groupName);
        }

        public async Task AddToGroup(string groupName)
        {
            await _socketDataAccess.AddToGroup(groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await _socketDataAccess.RemoveFromGroup(groupName);
        }

    }
}

