using Auction_API.BusinessUnit;
using Auction_API.DataAccess;
using Auction_API.Infrastructure;
using Auction_Project.BusinessUnit;
using Auction_Project.DataAccess;
using Auction_Project.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AuctionContext>(options =>
{
    var connectionStringBuilder = new ConnectionStringBuilder(builder.Configuration);
    options.UseNpgsql(connectionStringBuilder.Get());
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Identity ayarlarını yapılandırın
})
    .AddEntityFrameworkStores<AuctionContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IAuctionDataAccess, AuctionDataAccess>();
builder.Services.AddTransient<IAuctionBusinessUnit, AuctionBusinessUnit>();
builder.Services.AddTransient<IUserBusinessUnit,UserBusinessUnit>();
builder.Services.AddTransient<IUserDataAccess, UserDataAccess>();
builder.Services.AddTransient<IFavoriteBusinessUnit, FavoriteBusinessUnit>();
builder.Services.AddTransient<IFavoritesDataAccess, FavoritesDataAccess>();
builder.Services.AddTransient<IProductBusinessUnit, ProductBusinessUnit>();
builder.Services.AddTransient<IProductDataAccess, ProductDataAccess>();
builder.Services.AddTransient<IBidsBusinessUnit, BidsBusinessUnit>();
builder.Services.AddTransient<IBidsDataAccess, BidsDataAccess>();
builder.Services.AddTransient<ISocketBusinessUnit, SocketBusinessUnit>();
builder.Services.AddTransient<ISocketDataAccess, SocketDataAccess>();
builder.Services.AddTransient<IMessagesDataAccess, MessagesDataAccess>();
builder.Services.AddTransient<SignalRHub>(); // Scoped yerine Transient olarak kaydedin

// Timezone without TimeStamp türünde olduğu için aşağıdaki kod parçasını yazmak gerekli. Aksi taktirde hata alınacaktır.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().AllowAnonymous();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalRHub>("/SignalRHub");
    endpoints.MapControllers();
});

app.Run();
