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
using Microsoft.AspNetCore.Http;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using Npgsql;
using iText.Layout.Element;
using iText.Layout;
using Org.BouncyCastle.Crypto.Macs;

namespace DesigneryCore.Services
{
    public class GmailSmtpClientService//: IGmailSmtpClientService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587; // או 465 עבור SSL
        private readonly string gmailAddress;
        private readonly string gmailPassword;
        private readonly IConfiguration _config;
        private readonly string resetLinkBaseUrl = "https://engel.diversitech.co.il/#/myResetPasswordLink";


        public GmailSmtpClientService(string gmailAddress, string gmailPassword, IConfiguration configuration)
        {
            this.gmailAddress = gmailAddress;
            this.gmailPassword = gmailPassword;
            this._config = configuration;
        }

        public void SendEmail(string toAddress, string subject, string body, bool isBodyHtml = false, List<IFormFile> attachments = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Designery", gmailAddress));
            message.To.Add(new MailboxAddress("", toAddress));
            message.Subject = subject;

             var bodyBuilder = new BodyBuilder { HtmlBody = isBodyHtml ? body : null, TextBody = !isBodyHtml ? body : null };

             if (attachments != null && attachments.Any())
             {
                 foreach (var attachment in attachments)
                 {
                     if (attachment.Length > 0)
                     {
                         using (var stream = new MemoryStream())
                         {
                             attachment.CopyTo(stream);
                             stream.Position = 0;
                             bodyBuilder.Attachments.Add(attachment.FileName, stream.ToArray(), ContentType.Parse(attachment.ContentType));
                         }
                     }
                 }
             }

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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("no-reply@Designery.com", gmailAddress));
            message.To.Add(new MailboxAddress("", toAddress));
            message.Subject = "בקשה לאיפוס סיסמה";
            ///

            //check Email exists
            var u = DataAccessPostgreSQL.ExecuteFunction<User>("getuserbymail", [new NpgsqlParameter("p_mail", toAddress)]);
            if (u.Count() == 0)
            { throw new Exception(" Email dosn't exists "); }
            //מייל קיים

            var tokenService = new TokenService(_config);
            var token = tokenService.BuildAccessToken(
                "",
                toAddress
               );
            var linkResetPas = $"{resetLinkBaseUrl}/{token}";
 
            string body = $@"
    <div style='text-align: center; font-family: Arial, sans-serif; color: #4A4A4A;'>
        <h1 style='font-size: 28px; color: #E53D83;'>בהוקרה</h1>
        <h1 style='font-size: 24px; color: #555555;'>איפוס סיסמה</h1>
        <p style='font-size: 12px;'>קבלנו בקשה לאיפוס סיסמה עבור</p>
        <p style='font-size: 11px; font-weight: bold; color: #555555;'>{toAddress}</p>

        <a href='{linkResetPas}' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #ffffff; background-color: #FF007A; text-decoration: none; border-radius: 5px; margin-top: 20px;'>
            מעבר לאיפוס הסיסמה
        </a>
        <p style='font-size: 12px;'>קישור זה תקף ל 8 דקות</p>

    </div>";

            var bodyBuilder = new BodyBuilder { HtmlBody = body , TextBody = null };
                
                
                
                
               
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


        public void SendEmailPdf(string toAddress, string subject, string body, bool isBodyHtml = false, List<IFormFile> attachments = null, byte[] pdfAttachment = null, string pdfFileName = "OrderDetails.pdf")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("no-reply@Designery.com", gmailAddress));
            message.To.Add(new MailboxAddress("", toAddress));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = isBodyHtml ? body : null, TextBody = !isBodyHtml ? body : null };

            if (pdfAttachment != null)
            {
                bodyBuilder.Attachments.Add(pdfFileName, pdfAttachment, new ContentType("application", "pdf"));
            }

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
