﻿using Auction_Project.BusinessUnit;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AuctionContext>(options =>
{
    var connectionStringBuilder = new ConnectionStringBuilder(builder.Configuration);
    options.UseNpgsql(connectionStringBuilder.Get());
});
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AuctionContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // options here
})
    .AddEntityFrameworkStores<AuctionContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<IAuctionDataAccess, AuctionDataAccess>();
builder.Services.AddTransient<IAuctionBusinessUnit, AuctionBusinessUnit>();
builder.Services.AddTransient<UserBusinessUnit,UserBusinessUnit>();
builder.Services.AddTransient<IUserDataAccess, UserDataAccess>();
builder.Services.AddTransient<IFavoriteBusinessUnit, FavoriteBusinessUnit>();
builder.Services.AddTransient<IFavoritesDataAccess, FavoritesDataAccess>();
builder.Services.AddTransient<IBidsBusinessUnit, BidsBusinessUnit>();
builder.Services.AddTransient<IBidsDataAccess, BidsDataAccess>();


// Timezone without TimeStamp türünde olduğu için aşağıdaki kod parçasını yazmak gerekli. Aksi taktirde hata alınacaktır.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAuthentication();

app.Run();

