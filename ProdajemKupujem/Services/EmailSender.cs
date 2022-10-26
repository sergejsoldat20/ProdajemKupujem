using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace ProdajemKupujem.Services
{
    public class EmailSender : IEmailSender
    {

        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;

        public EmailSender()
        {
            using (StreamReader r = new StreamReader("email.json"))
            {
                string json = r.ReadToEnd();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                this.host = dictionary["host"];
                this.port = Int32.Parse(dictionary["port"]);
                this.enableSSL = bool.Parse(dictionary["enableSSL"]);
                this.userName = dictionary["userName"];
                this.password = dictionary["password"];
            }
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(this.userName, this.password),
                EnableSsl = enableSSL
            };
             return client.SendMailAsync(new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true});
        }
    }
}
