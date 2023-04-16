namespace Auction_Project.Infrastructure;

public interface IServiceResponse
{
    ResponseCode ResponseCode { get; }
    string Message { get; }
}