using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

namespace ProdajemKupujem.Models
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Reciever { get; set; }
        public bool IsSeen { get; set; }
    }
}
