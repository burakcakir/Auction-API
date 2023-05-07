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
using Auction_API.Infrastructure.Dto;
using Auction_Project.Infrastructure.Entity;

namespace Auction_Project.BusinessUnit;

public interface IUserBusinessUnit
{
    Task<Response> AddNewUser(UserAddDto userAddInput);
    Task<Response> CreateRole(string roleName);
    Task<UserLoginDto> UserLogin(string useremail, string password);
    Task<Response> DeleteUser(int userId);
    Task<Response> UpdateUser(UserUpdateDto userUpdateInput);
    Task<Response> ChangePassword(string useremail, string password);
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

    public async Task<Response> AddNewUser(UserAddDto userAddInput)
    {
        var userStore = new UserStore<IdentityUser>(_context);
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var userManager = new UserManager<IdentityUser>(userStore, null, passwordHasher, null, null, null, null, null, null);
        var user = new IdentityUser { UserName = userAddInput.UserName.ToUpper(), Email = userAddInput.Email.ToUpper() };
        var result = await userManager.CreateAsync(user, userAddInput.Password);

        if (result.Succeeded)
        {
            var newEntity = new User
            {
                UserName = userAddInput.UserName,
                Name = userAddInput.Name,
                Surname = userAddInput.Surname,
                Password = userAddInput.Password,
                PhoneNumber = userAddInput.PhoneNumber,
                IdentityNumber = userAddInput.IdentityNumber,
                Address = userAddInput.Address,
                Email = userAddInput.Email
            };

            _userDataAccess.Add(newEntity);
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

    public async Task<UserLoginDto> UserLogin(string email, string password)
    {
        var output = new UserLoginDto();

        var user = await _userManager.FindByEmailAsync(email.ToUpper());

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

                output.userAccesToken = tokenString;
                output.response = new Response(ResponseCode.Success, "Kullanıcı Girişi Başarılı");
                output.userInformation = _userDataAccess.GetUserByUsername(user.UserName.ToLower());
                return output;
            }
            else
            {
                output.userAccesToken = string.Empty;
                output.response = new Response(ResponseCode.Fail, "Kullanıcı Girişi Başarısız");
                output.userInformation = null;
                return output;
            }
        }
        else
        {
            output.userAccesToken = string.Empty;
            output.response = new Response(ResponseCode.NotFound, "Kullanıcı Bulunamadı");
            output.userInformation = null;
            return output;
        }

    }

    public async Task<Response> DeleteUser(int userId)
    {
        var userEntity = _userDataAccess.GetUserByUserId(userId);
        if (userEntity == null)
        {
            return new Response(ResponseCode.Fail, "User Entity is null");
        }

        var deleteChanges = await _userDataAccess.Delete(userEntity);
        if (deleteChanges > 0)
        {
            return new Response(ResponseCode.Success, "success");
        }
        return new Response(ResponseCode.Fail, "Silme işlemi başarısız");
    }

    public async Task<Response> UpdateUser(UserUpdateDto userUpdateInput)
    {
        try
        {
            var userEntity = _userDataAccess.GetUserByUserId(userUpdateInput.Id);

            if (userEntity != null)
            {
                var userCoreIdentityTableDatas = await _userManager.FindByEmailAsync(userEntity.Email.ToUpper());

                if (userCoreIdentityTableDatas != null && userEntity.Email != userUpdateInput.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(userCoreIdentityTableDatas, userUpdateInput.Email.ToUpper());
                    if (!setEmailResult.Succeeded)
                    {
                        return new Response(ResponseCode.Fail, "Fail");
                    }
                }

                userEntity.Name = userUpdateInput.Name;
                userEntity.Surname = userUpdateInput.Surname;
                userEntity.PhoneNumber = userUpdateInput.PhoneNumber;
                userEntity.IdentityNumber = userUpdateInput.IdentityNumber;
                userEntity.Address = userUpdateInput.Address;
                userEntity.Email = userUpdateInput.Email;


                var saveChangesValue = await _userDataAccess.Update(userEntity);
                if (saveChangesValue > 0)
                    return new Response(ResponseCode.Success, "User Updated Successully");
                else
                    return new Response(ResponseCode.Fail, "User Update Not Successfully");
            }
            else
            {
                return new Response(ResponseCode.Fail, "User Not Found");
            }
        }
        catch (Exception)
        {
            return new Response(ResponseCode.Fail, "Exception");
        }
    }

    public async Task<Response> ChangePassword(string email,string password)
    {
        try
        {
            var userEntity = _userDataAccess.GetUserByEmail(email);

            if (userEntity != null)
            {
                var userCoreIdentityTableDatas = await _userManager.FindByEmailAsync(email.ToUpper());

                if (userCoreIdentityTableDatas != null)
                {
                    var setEmailResult = await _userManager.ChangePasswordAsync(userCoreIdentityTableDatas, userEntity.Password, password);
                    if (!setEmailResult.Succeeded)
                    {
                        string CreateResponseMessage = setEmailResult.Errors.ElementAt(0).Description;

                        return new Response(ResponseCode.Fail, CreateResponseMessage);
                    }
                    else
                    {
                        if (userEntity != null)
                        {
                            userEntity.Password = password;

                            var saveChangesValue = await _userDataAccess.Update(userEntity);

                            if (saveChangesValue > 0)
                                return new Response(ResponseCode.Success, "Password Changed Successully");
                            else
                                return new Response(ResponseCode.Fail, "Password Changed Not Successfully");
                        }

                        return new Response(ResponseCode.Success, "User Password Changed Successully");
                    }
                }
                else
                {
                    return new Response(ResponseCode.Fail, "User Not Found in CoreIdentity");
                }
            }
            else
            {
                return new Response(ResponseCode.Fail, "User Not Found");
            }
        }
        catch (Exception)
        {
            return new Response(ResponseCode.Fail, "Exception");
        }
    }

}