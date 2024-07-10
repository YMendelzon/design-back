using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.AspNetCore.Authorization;
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

