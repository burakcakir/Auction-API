namespace Auction_Project.Infrastructure.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }
    
    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public string IdentityNumber { get; set; }

    public string Address { get; set; }

    public string Email  { get; set; }
}