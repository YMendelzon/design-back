namespace DesigneryCommon.Models
{
    public class EmailRequest
    {
        public string Greeting { get; set; } // ברכה מותאמת אישית

        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
