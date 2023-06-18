using System.Linq.Expressions;
using Auction_API.Infrastructure.Entity;
using Auction_Project.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auction_Project.Infrastructure;

public class AuctionContext : IdentityDbContext
{
    public AuctionContext()
    {

    }

    public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
    {
    }

    public virtual DbSet<Auction> Auction { get; set; } = null!;

    public virtual DbSet<Bids> Bids { get; set; } = null!;

    public virtual DbSet<Favorite> Favorites { get; set; } = null!;

    public virtual DbSet<Messages> Messages { get; set; } = null!;

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<User> Users { get; set; } = null!;

    public virtual DbSet<SignalRGroups> SignalRGroups { get; set; } = null!;

    public virtual DbSet<SignalRGroupUsers> SignalRGroupUsers { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);

            }
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Modified
                || e.State == EntityState.Deleted));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.State = EntityState.Unchanged;
                ((BaseEntity)entityEntry.Entity).IsDeleted = true;
            }

        }

        return await base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionStringBuilder = new ConnectionStringBuilder();
            optionsBuilder.UseNpgsql(connectionStringBuilder.Get());
        }
    }
}