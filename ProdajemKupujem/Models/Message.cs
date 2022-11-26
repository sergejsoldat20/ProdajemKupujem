using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

namespace ProdajemKupujem.Models
{
    public class Message : BaseModel
    {
        public static string GetChatId(int senderId, int receiverId)
            => senderId < receiverId ? $"{senderId}-{receiverId}" : $"{receiverId}-{senderId}";

        public Message() { }
        public Message(string text, int senderId, int receiverId, ApplicationUser sender, ApplicationUser receiver, bool isSeen = false)
        {
            Text = text;
            SenderId = senderId;
            ReceiverId = receiverId;
            ChatId = Message.GetChatId(senderId, receiverId);
            Sender = sender;
            Receiver = receiver;
            IsSeen = isSeen;
        }

        public string Text { get; set; }
        public int SenderId { get; private set; }
        public int ReceiverId { get; private set; }
        public string ChatId { get; set; }
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
        public bool IsSeen { get; set; }
    }
}
