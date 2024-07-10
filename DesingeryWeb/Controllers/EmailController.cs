using DesigneryCommon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly GmailSmtpClientService _gmailSmtpClient;

        public EmailController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            // Initialize GmailSmtpClient
            string gmailAddress = configuration["Gmail:Address"];
            string gmailPassword = configuration["Gmail:Password"];
            _gmailSmtpClient = new GmailSmtpClientService(gmailAddress, gmailPassword);
        }

        
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequest emailRequest)
        {
            try
            {
                _gmailSmtpClient.SendEmail(emailRequest.ToAddress, emailRequest.Subject, emailRequest.Body, emailRequest.IsBodyHtml, emailRequest.Attachments);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error sending email: {ex.Message}");
            }
        }

        [HttpPut("add-data")]
        public IActionResult AddDataEntry([FromBody] DataEntry dataEntry)
        {
            if (dataEntry == null)
            {
                return BadRequest("Data entry is null.");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // הוספת Name ו-Email לטבלה DataEntries
                string query1 = "INSERT INTO DataEntries (Name, Email) OUTPUT INSERTED.Id VALUES (@Name, @Email)";
                SqlCommand command1 = new SqlCommand(query1, connection);
                command1.Parameters.AddWithValue("@Name", dataEntry.Name);
                command1.Parameters.AddWithValue("@Email", dataEntry.Email);

                // קבלת מזהה השורה החדשה שנוספה
                int dataEntryId = (int)command1.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(dataEntry.Message))
                {
                    // הוספת Message לטבלה Messages עם מזהה ה-DataEntry
                    string query2 = "INSERT INTO Messages (Message, DataEntryId) VALUES (@Message, @DataEntryId)";
                SqlCommand command2 = new SqlCommand(query2, connection);
                command2.Parameters.AddWithValue("@Message", dataEntry.Message);
                command2.Parameters.AddWithValue("@DataEntryId", dataEntryId);

                command2.ExecuteNonQuery();
                }
            }

            return Ok("Data entry added successfully.");
        }


        [HttpPost("send-emails")]
        public async Task<IActionResult> SendEmails([FromForm] EmailRequest emailRequest)
        {
            if (emailRequest == null || emailRequest.EmailList == null || !emailRequest.EmailList.Any())
            {
                return BadRequest("Email request or email list is null or empty.");
            }

            foreach (string email in emailRequest.EmailList)
            {
                string personalizedBody = $"{emailRequest.Greeting},\n\n{emailRequest.Body}";

                try
                {
                    _gmailSmtpClient.SendEmail(email, emailRequest.Subject, personalizedBody, emailRequest.IsBodyHtml, emailRequest.Attachments=null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                }
            }

            return Ok("Emails sent successfully.");
        }
        /*[HttpPost("send-emails")]
        public async Task<IActionResult> SendEmails([FromForm] EmailRequest emailRequest)
        {
            if (emailRequest == null || emailRequest.EmailList == null || !emailRequest.EmailList.Any())
            {
                return BadRequest("Email request or email list is null or empty.");
            }

            foreach (string email in emailRequest.EmailList)
            {
                string personalizedBody = $"{emailRequest.Greeting},\n\n{emailRequest.Body}";

                try
                {
                    // שליחת אימייל עם או בלי Attachments
                    _gmailSmtpClient.SendEmail(
                        email,
                        emailRequest.Subject,
                        personalizedBody,
                        emailRequest.IsBodyHtml,
                        emailRequest.Attachments // יכול להיות null
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                }
            }

            return Ok("Emails sent successfully.");






        }*/
}


