/*using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace DesingeryWeb.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly GmailSmtpClientService _gmailSmtpClient;

        public EmailController(IConfiguration configuration)
        {
            string gmailAddress = configuration["Gmail:Address"];
            string gmailPassword = configuration["Gmail:Password"];
            _gmailSmtpClient = new GmailSmtpClientService(gmailAddress, gmailPassword);
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            _gmailSmtpClient.SendEmail(emailRequest.ToAddress, emailRequest.Subject, emailRequest.Body, emailRequest.IsBodyHtml);
            return Ok("Email sent successfully.");
        }
    }
}
*/


/////////////////////////////////////////////////////////////////////////////////////
///


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
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null)
            {
                return BadRequest("Email request is null.");
            }

            _gmailSmtpClient.SendEmail(emailRequest.ToAddress, emailRequest.Subject, emailRequest.Body, emailRequest.IsBodyHtml);
            return Ok("Email sent successfully.");
        }

        [HttpPut("add-data")]
        public IActionResult AddDataEntry([FromBody] EmailRequest dataEntry)
        {
            if (dataEntry == null)
            {
                return BadRequest("Data entry is null.");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO DataEntries (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", dataEntry.Name);
                command.Parameters.AddWithValue("@Email", dataEntry.Email);
                command.Parameters.AddWithValue("@Message", dataEntry.Message);

                command.ExecuteNonQuery();
            }

            return Ok("Data entry added successfully.");
        }

        [HttpPost("send-emails")]
        public IActionResult SendEmails([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null)
            {
                return BadRequest("Email request is null.");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // שליפת שמות וכתובות האימייל
                string query = "SELECT Name, Email FROM DataEntries";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // שליחת הודעה לכל כתובת
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string email = reader.GetString(1);

                    // עדכון ההודעה עם השם והברכה
                    string personalizedBody = $"{emailRequest.Greeting} {name},\n\n{emailRequest.Body}";

                    try
                    {
                        _gmailSmtpClient.SendEmail(email, emailRequest.Subject, personalizedBody, emailRequest.IsBodyHtml);
                    }
                    catch (Exception ex)
                    {
                        // רישום החריגות או טיפול בהתאם
                        Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                    }
                }
            }

            return Ok("Emails sent successfully.");
        }


    }
}


