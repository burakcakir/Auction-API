using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auction_Project.Infrastructure.Entity;

namespace Auction_API.Infrastructure
{
    public class SignalRHub : Hub
    {
        public async Task SendMessageToAll(string message) //Uygulamayı kullanan bütün kullanıcılara mesaj göndermek için kullanılır
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessagePrivate(string connectionId,string message) //Kullanıcı ve Satıcı arasındaki private mesajlaşmalarda kullanılır.
        {
            await Clients.Clients(connectionId).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToGroup(string groupName, string message) //Sadece belli bir gruba dahil olan kullanıcılara mesaj göndermek için kullanılır.
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task CreateGroup(string groupName) //Hub üzerinde grup oluşturmak için kullanılır.
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task AddToGroup(string groupName) //İlgili kullanıcıyı bir gruba eklemek için kullanılır.
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName) //İlgili kullanıcıyı gruptan çıkartmak için kullanılır.
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override Task OnConnectedAsync() //User hub'a connect olduğunda çalışır.
        {
            string connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception exception) //User'ın hub üzerindeki bağlantısı koptuğu zaman çalışır.
        {
            string connectionId = Context.ConnectionId;
            return base.OnDisconnectedAsync(exception);
        }

    }
}