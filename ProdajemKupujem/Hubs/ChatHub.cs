using Microsoft.AspNetCore.SignalR;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using System.Threading.Tasks;

namespace ProdajemKupujem.Hubs
{
    public class ChatHub : Hub
    {
        //define a dictionary to store the userid.
        private static Dictionary<string, List<string>> NtoIdMappingTable = new Dictionary<string, List<string>>();

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            var userId = Context.ConnectionId;
            List<string> userIds;

            //store the userid to the list.
            if (!NtoIdMappingTable.TryGetValue(username, out userIds))
            {
                userIds = new List<string>();
                userIds.Add(userId);

                NtoIdMappingTable.Add(username, userIds);
            }
            else
            {
                userIds.Add(userId);
            }

            await base.OnConnectedAsync();
        }
        public async Task SendMessageToAll(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {

            var userId = NtoIdMappingTable.GetValueOrDefault(receiver);
            await Clients.User(userId.FirstOrDefault()).SendAsync("ReceiveMessage", sender, message);
       
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            var userId = Context.ConnectionId;
            List<string> userIds;

            //remove userid from the List
            if (NtoIdMappingTable.TryGetValue(username, out userIds))
            {
                userIds.Remove(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
           

