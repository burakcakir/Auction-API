using Npgsql;

namespace Auction_Project.Infrastructure;

public class ConnectionStringBuilder
{
    private readonly IConfiguration _configuration;

    public ConnectionStringBuilder(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string Get()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "mezat-db.cmdkoclkeofc.eu-north-1.rds.amazonaws.com",
            Port = 5432,
            Username = "mezatuser",
            Password = "Mezat123456",
            Database = "postgres"
        };
        
        return connStringBuilder.ConnectionString;
    }
}
