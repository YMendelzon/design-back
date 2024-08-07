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
        //[HttpPost("SendEmailToRest")]
        //public IActionResult sendLinkResetPas(string toAddress)
        //{
        //    _gmailSmtpClient.SendEmailToRest(toAddress);
        //    return Ok("Email sent successfully.");
        //}
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
using Npgsql;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly GmailSmtpClientService _gmailSmtpClient;
        private readonly IPdfGeneratorService _pdfGeneratorService;


        public EmailController(IConfiguration configuration, IPdfGeneratorService pdfGeneratorService)
        {
            //_connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = configuration.GetConnectionString("PostgreSqlConnection");


            // Initialize GmailSmtpClient
            string gmailAddress = configuration["Gmail:Address"];
            string gmailPassword = configuration["Gmail:Password"];
            _gmailSmtpClient = new GmailSmtpClientService(gmailAddress, gmailPassword, configuration);
            _pdfGeneratorService = pdfGeneratorService;
        }

        /*[HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null)
            {
                return BadRequest("Email request is null.");
            }

            _gmailSmtpClient.SendEmail(emailRequest.ToAddress, emailRequest.Subject, emailRequest.Body, emailRequest.IsBodyHtml, emailRequest.Attachments);
            return Ok("Email sent successfully.");
        }*/


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

        [HttpPost("sendToResetPas")]
        public async Task<IActionResult> sendToResetPas(string ToAddress)
        {
            try
            {
                _gmailSmtpClient.SendEmailToRest(ToAddress);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error sending email: {ex.Message}");
            }
        }

        /*[HttpPut("add-data")]
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

                

                if (!(string.IsNullOrWhiteSpace(dataEntry.Message)))
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
        }*/

        [HttpPut("add-data")]
        public async Task<IActionResult> AddDataEntry([FromBody] DataEntry dataEntry)
        {
            if (dataEntry == null)
            {
                return BadRequest("Data entry is null.");
            }
            if (string.IsNullOrWhiteSpace(dataEntry.Message))
            {
                // Do nothing if Message is empty or whitespace
                return Ok("Message is empty. No data entry was added.");
            }
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT add_data_entry(@Name, @Email, @Message)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", dataEntry.Name);
                        command.Parameters.AddWithValue("@Email", dataEntry.Email);
                        command.Parameters.AddWithValue("@Message", dataEntry.Message);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok("Data entry added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpPost("sendPdf")]
        public async Task<IActionResult> SendEmailPDF([FromForm] EmailRequest emailRequest)
        {
            try
            {
                byte[] pdfBytes = _pdfGeneratorService.GenerateOrderDetailsPdf();

                _gmailSmtpClient.SendEmailPdf(emailRequest.ToAddress, emailRequest.Subject, emailRequest.Body, emailRequest.IsBodyHtml, emailRequest.Attachments, pdfBytes);
                    return Ok("Email sent successfully.");
                

                //return File(pdfBytes, "application/pdf", $"Order_.pdf");
             
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error sending email: {ex.Message}");
            }
        }

      

        //[HttpPut("add-data")]
        //public async Task<IActionResult> AddDataEntry([FromBody] EmailRequest dataEntry)
        //{
        //    if (dataEntry == null)
        //    {
        //        return BadRequest("Data entry is null.");
        //    }

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        string query = "INSERT INTO DataEntries (Name, Email, Message) VALUES (@Name, @Email, @Message)";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@Name", dataEntry.Name);
        //        command.Parameters.AddWithValue("@Email", dataEntry.Email);
        //        command.Parameters.AddWithValue("@Message", dataEntry.Message);

        //        await command.ExecuteNonQueryAsync();
        //    }

        //    return Ok("Data entry added successfully.");
        //}


        //[HttpPost("send-emails")]
        //public IActionResult SendEmails([FromBody] EmailRequest emailRequest)
        //{
        //    if (emailRequest == null)
        //    {
        //        return BadRequest("Email request is null.");
        //    }

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // שליפת שמות וכתובות האימייל
        //        string query = "SELECT Name, Email FROM DataEntries";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        SqlDataReader reader = command.ExecuteReader();

        //        // שליחת הודעה לכל כתובת
        //        while (reader.Read())
        //        {
        //            string name = reader.GetString(0);
        //            string email = reader.GetString(1);

        //            // עדכון ההודעה עם השם והברכה
        //            string personalizedBody = $"{emailRequest.Greeting} {name},\n\n{emailRequest.Body}";

        //            try
        //            {
        //                _gmailSmtpClient.SendEmail(email, emailRequest.Subject, personalizedBody, emailRequest.IsBodyHtml);
        //            }
        //            catch (Exception ex)
        //            {
        //                // רישום החריגות או טיפול בהתאם
        //                Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
        //            }
        //        }
        //    }

        //    return Ok("Emails sent successfully.");
        //}
        [HttpPost("send-emails")]
        public async Task<IActionResult> SendEmails([FromForm] EmailRequest emailRequest)
        {
            if (emailRequest == null)
           {
                return BadRequest("Email request is null.");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Name, Email FROM DataEntries";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())

                {
                    string name = reader.GetString(0);
                    string email = reader.GetString(1);

                    string personalizedBody = $"{emailRequest.Greeting} {name},\n\n{emailRequest.Body}";

                    try
                   {
                       _gmailSmtpClient.SendEmail(email, emailRequest.Subject, personalizedBody, emailRequest.IsBodyHtml, emailRequest.Attachments);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
                   }
              }
           }

            return Ok("Emails sent successfully.");
        }
        //[HttpPost("send-emails")]
        //public async Task<IActionResult> SendEmails([FromForm] EmailRequest emailRequest)
        //{
        //    if (emailRequest == null || emailRequest.EmailList == null || !emailRequest.EmailList.Any())
        //    {
        //        return BadRequest("Email request or email list is null or empty.");
        //    }

        //    foreach (string email in emailRequest.EmailList)
        //    {
        //        string personalizedBody = $"{emailRequest.Greeting},\n\n{emailRequest.Body}";

        //        try
        //        {
        //            _gmailSmtpClient.SendEmail(email, emailRequest.Subject, personalizedBody, emailRequest.IsBodyHtml, emailRequest.Attachments);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
        //        }
        //    }

        //    return Ok("Emails sent successfully.");
        //}


    }




}
    



