using System.Net.Mail;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using DesigneryCore.Interfaces;
using System.Data.SqlClient;

namespace DesigneryCore.Services
{
    public class GmailSmtpClientService//: IGmailSmtpClientService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587; // או 465 עבור SSL
        private readonly string gmailAddress;
        private readonly string gmailPassword;

        public GmailSmtpClientService(string gmailAddress, string gmailPassword)
        {
            this.gmailAddress = gmailAddress;
            this.gmailPassword = gmailPassword;
        }

        public void SendEmail(string toAddress, string subject, string body, bool isBodyHtml = false)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", gmailAddress));
            message.To.Add(new MailboxAddress("", toAddress));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = isBodyHtml ? body : null, TextBody = !isBodyHtml ? body : null };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(gmailAddress, gmailPassword);
                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
