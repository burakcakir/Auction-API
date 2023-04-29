using Auction_Project.Infrastructure.Entity;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auction_Project.BusinessUnit;

namespace Auction_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBusinessUnit _userBusinessUnit;

        public UserController(UserBusinessUnit userBusinessUnit)
        {
            _userBusinessUnit = userBusinessUnit;
        }

        [HttpPost]
        [Route("AddUserAsync")]
        public async Task<Response> AddUserAsync(string userName, string password)
        {
            return await _userBusinessUnit.AddNewUser(userName, password);
        }


        [HttpPost]
        [Route("CreateRole")]
        public async Task<Response> CreateRole(string roleName)
        {
            return await _userBusinessUnit.CreateRole(roleName);
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<Response> UserLogin(string username,string password)
        {
            return await _userBusinessUnit.UserLogin(username,password);
        }
    }
}
