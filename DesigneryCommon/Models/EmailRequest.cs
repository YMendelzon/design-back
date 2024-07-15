using Microsoft.AspNetCore.Http;

namespace DesigneryCommon.Models
{
    public class EmailRequest
    {
        public string? Greeting { get; set; } // ברכה מותאמת אישית

        public string? ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        
        public List<IFormFile>? Attachments { get; set; }
        public List<string>? EmailList { get; set; }


    }
}
