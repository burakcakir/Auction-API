namespace Auction_Project.Infrastructure;

public enum ResponseCode
{
    Success = 200,
    NoContent = 204,
    BadRequest = 400,
    UnAuthorized = 401,
    NotFound = 404,
    Fail = 500,
}