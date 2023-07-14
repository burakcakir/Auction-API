using Auction_Project.Infrastructure.Entity;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auction_Project.BusinessUnit;
using Auction_API.Infrastructure.Dto;

namespace Auction_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessUnit _userBusinessUnit;

        public UserController(IUserBusinessUnit userBusinessUnit)
        {
            _userBusinessUnit = userBusinessUnit;
        }

        [HttpPost]
        [Route("AddUserAsync")]
        public async Task<Response> AddUserAsync([FromBody] UserAddDto input)
        {
            return await _userBusinessUnit.AddNewUser(input);
        }


        [HttpPost]
        [Route("CreateRole")]
        public async Task<Response> CreateRole(string roleName)
        {
            return await _userBusinessUnit.CreateRole(roleName);
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<UserLoginDto> UserLogin([FromBody] UserLoginRequestDto requestDto)
        {
            return await _userBusinessUnit.UserLogin(requestDto.UserEmail,requestDto.Password);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<Response> DeleteUser()
        {
            return await _userBusinessUnit.DeleteUser();
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<Response> UpdateUser([FromBody] UserUpdateDto input)
        {
            return await _userBusinessUnit.UpdateUser(input);
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<Response> ChangePassword([FromBody] ChangePasswordInput input)
        {
            return await _userBusinessUnit.ChangePassword(input.UserEmail, input.Password);
        }

        [HttpGet]
        [Route("GetSingleUser")]
        public async Task<GetUserOutput> GetUserInformation()
        {
            return await _userBusinessUnit.GetUserInformation();
        }
        
        [HttpGet]
        [Route("GetUserId")]
        public async Task<string> GetUserId()
        {
            return await _userBusinessUnit.GetUserId();
        }
        
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<User> GetUserProfile(int userId)
        {
            return await _userBusinessUnit.GetUserProfile(userId);
        }
    }
}
