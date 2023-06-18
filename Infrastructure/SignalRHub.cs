using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Auction_API.Infrastructure
{
    public class SignalRHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task AddUserToGroup(string groupName, string userId)
        {
            await Clients.Group(groupName).SendAsync("UserAdded", userId);
        }

        public async Task CreateGroup(string groupName)
        {
            await Clients.All.SendAsync("CreateGroup", groupName);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync(message);
        }

        /*

            1 - Grupları tutmak için db'de tablo oluştur
            2 - Gruplara eklenen kullanıcıları listelemek için bir tablo oluştur
            3 - API katmanına bir adet controller oluştur
            4 - Controller içerisinde ;
            4.1 -> Kullanıcı ve atıcı arası birebir mesajlaşma
            4.2 -> kullanıcının favorilere eklediği veya teklif verdiği bid'ler hakkında bildirim alması için metod oluşturma
         
        */
    }
}