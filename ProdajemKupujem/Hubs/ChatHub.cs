using Microsoft.AspNetCore.SignalR;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using System.Threading.Tasks;

namespace ProdajemKupujem.Hubs
{
    public class ChatHub : Hub
    {
        //define a dictionary to store the userid.
        private static Dictionary<string,string> UserToConnectionIdMap = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            var userId = Context.UserIdentifier;

            UserToConnectionIdMap.Add(username, userId);

            await base.OnConnectedAsync();
        }
        public async Task SendMessageToAll(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public async Task SendMessage(string user, string message, string receiver)
        {
            var userId = UserToConnectionIdMap.GetValueOrDefault(receiver);
            if (userId != null)
            {
                await Clients.User(userId).SendAsync("ReceiveMessage", user, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            if (UserToConnectionIdMap.ContainsValue(username))
            {
                UserToConnectionIdMap.Remove(username);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
           

