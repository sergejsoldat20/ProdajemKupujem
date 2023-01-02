using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

namespace ProdajemKupujem.Models
{
    public class Message : BaseModel
    {
        public Message()
        {
            SenderId = "";
            ReceiverId = "";
        }
        public string SenderId { get; private set; }
        public string ReceiverId { get; private set; }

        public void SetSenderId(string senderId)
        {
            this.SenderId = senderId;
            this.ChatId = Message.GenerateChatId(this.SenderId, this.ReceiverId);
        }

        public void SetReceiverId(string receiverId)
        {
            this.ReceiverId = receiverId;
            this.ChatId = Message.GenerateChatId(this.SenderId, this.ReceiverId);
        }

        public string Text { get; set; } = "";
        public string ChatId { get; private set; } = "";

        public static string GenerateChatId(string SenderId, string ReceiverId)
        {
            if (String.Compare(SenderId, ReceiverId) < 0)
            {
                return SenderId.ToString() + ";" + ReceiverId.ToString();
            }
            else
            {
                return ReceiverId.ToString() + ";" + SenderId.ToString();
            }
        }
    }
}
