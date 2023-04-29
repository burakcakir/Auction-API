using System;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auction_Project.BusinessUnit;

public interface IUserBusinessUnit
{
    Task<Response> AddNewUser(string username, string password);
    Task<Response> CreateRole(string roleName);
    Task<Response> UserLogin(string username, string password);
}

public class UserBusinessUnit : IUserBusinessUnit
{
    private readonly IUserDataAccess _userDataAccess;

    private readonly AuctionContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    public UserBusinessUnit(AuctionContext context, IUserDataAccess userDataAccess, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _context = context;
        _userDataAccess = userDataAccess;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;

    }

    public async Task<Response> AddNewUser(string username, string password)
    {
        var userStore = new UserStore<IdentityUser>(_context);
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var userManager = new UserManager<IdentityUser>(userStore, null, passwordHasher, null, null, null, null, null, null);
        var user = new IdentityUser { UserName = username };
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return new Response(ResponseCode.Success, "Success");
        }
        else
        {
            return new Response(ResponseCode.Success, "Bad request");
        }
    }

    public async Task<Response> CreateRole(string roleName)
    {
        var serviceProvider = _httpContextAccessor.HttpContext.RequestServices;

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var role = new IdentityRole(roleName);
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new Response(ResponseCode.Success, "Success");
            }
        }

        return new Response(ResponseCode.Success, "Bad request");
    }

    public async Task<Response> UserLogin(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {
                return new Response(ResponseCode.Success, "Success");
            }
            else
            {
                return new Response(ResponseCode.Fail, "UnSuccess");
            }
        }
        else
        {
            return new Response(ResponseCode.NoContent, "No user");
        }

    }



}