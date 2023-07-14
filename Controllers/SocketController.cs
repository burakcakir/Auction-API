using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction_API.BusinessUnit;
using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auction_API.Controllers
{
    [Route("socket/[controller]")]
    public class SocketController : Controller
    {
        private readonly ISocketBusinessUnit _socketBusinessUnit;

        public SocketController(ISocketBusinessUnit socketBusinessUnit)
        {
            _socketBusinessUnit = socketBusinessUnit;
        }

        [HttpPost]
        [Route("SendMessageAsync")]
        public async Task<Response> SendMessageAsync([FromBody] SocketSendMessageInput input)
        {
            return await _socketBusinessUnit.SendMessageAsync(input.userId,input.message);
        }
    }
}
