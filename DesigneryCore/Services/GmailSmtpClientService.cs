using System.Net.Mail;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using DesigneryCore.Interfaces;
using DesigneryCommon.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;
using DesigneryDAL;
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
            message.From.Add(new MailboxAddress("no-reply@Designery.com", gmailAddress));
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
        public async void SendEmailToRest(string toAddress)//, string subject, string body, bool isBodyHtml = false)
        {
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("", gmailAddress));
            //message.To.Add(new MailboxAddress("", toAddress));
            //message.Subject = subject;

            //var bodyBuilder = new BodyBuilder { HtmlBody = isBodyHtml ? body : null, TextBody = !isBodyHtml ? body : null };
            //message.Body = bodyBuilder.ToMessageBody();
            ////check Email exists
            //var u = DataAccess.ExecuteStoredProcedure<User>("ExistingUser", [new SqlParameter("@email", toAddress)]);
            //if (u.Count() > 0) 
            //{

            //}
            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var resetLink = Url.Action("ResetPassword", "Account", new { token = token, email = model.Email }, Request.Scheme);


            //try
            //{
            //    using (var client = new SmtpClient())
            //    {
            //        client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            //        client.Authenticate(gmailAddress, gmailPassword);
            //        client.Send(message);
            //        client.Disconnect(true);
            //        Console.WriteLine("Email sent successfully.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception: {ex.Message}");
            //}
        }
    }
}
