using Microsoft.AspNetCore.SignalR;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using System.Threading.Tasks;

namespace ProdajemKupujem.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<string,string> UserToConnectionIdMap = new Dictionary<string, string>();
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

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

                var sender = await _context.Users.FindAsync(user);
                var receiverUser = await _context.Users.FindAsync(receiver);

                ///TODO: save to database
                //Message newMessage = new(message, user, receiver, sender, receiverUser);
                //newMessage.SenderId = user;
                //newMessage.RecieverId = receiver;

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
           

