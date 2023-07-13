using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Auction_API
{
    public class LambdaFunction : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Program>()  // Startup sınıfını burada belirtin
                .UseApiGateway();
        }
    }
}
