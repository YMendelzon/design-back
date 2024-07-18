using DesigneryCommon.Models;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesigneryWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailchimpController : ControllerBase
    {
        private readonly MailchimpService _mailchimpService;

        public MailchimpController(MailchimpService mailchimpService)
        {
            _mailchimpService = mailchimpService;
        }

        [HttpPost("add-subscriber")]
        public async Task<IActionResult> AddSubscriber([FromBody] SubscribeRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ListId) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                // הוספת לוגים לבדיקת הנתונים הנשלחים
                Console.WriteLine($"ListId: {request.ListId}");
                Console.WriteLine($"Email: {request.Email}");
                Console.WriteLine($"Name: {request.FName}");

                await _mailchimpService.AddEmailToListAsync(request.Email, request.FName);
                return Ok(new { message = "Subscriber added successfully." });
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)e.StatusCode, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("subscribers/{listId}")]
        public async Task<IActionResult> GetSubscribers(string listId)
        {
            try
            {
                var subscribers = await _mailchimpService.GetSubscribersAsync();
                return Ok(subscribers);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)e.StatusCode, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("lists")]
        public async Task<IActionResult> GetLists()
        {
            try
            {
                var lists = await _mailchimpService.GetListsAsync();
                return Ok(lists);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)e.StatusCode, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
        [HttpPost("send-campaign")]
        public async Task<IActionResult> SendCampaign([FromBody] SendCampaignRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Subject) || string.IsNullOrWhiteSpace(request.HtmlContent))
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                var campaignId = await _mailchimpService.CreateCampaignAsync(request.Subject);
                await _mailchimpService.AddContentToCampaignAsync(campaignId, request.HtmlContent);
                await _mailchimpService.SendCampaignAsync(campaignId);
                return Ok(new { message = "Campaign sent successfully." });
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)e.StatusCode, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

    }
}