using System;
using Auction_API.Infrastructure.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
    private readonly IConfiguration _configuration;

    public UserBusinessUnit(AuctionContext context, IUserDataAccess userDataAccess, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _context = context;
        _userDataAccess = userDataAccess;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
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
                // JWT ayarlarını yükleyin
                var jwtSettingsSection = _configuration.GetSection("JwtSettings");
                var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenLifetimeInMinutes)
                                            .AddSeconds(new Random(DateTime.Now.Millisecond).Next(-30, 30)), // Rastgele bir zaman aralığı ekle
                    SigningCredentials = creds,
                    Issuer = jwtSettings.Issuer,
                    Audience = jwtSettings.Audience
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Token'ı depola
                var tokenName = "UserLoginToken";
                var tokenValue = tokenString;
                await _userManager.SetAuthenticationTokenAsync(user, "AuctionApp", tokenName, tokenValue);

                return new Response(ResponseCode.Success, tokenString);
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