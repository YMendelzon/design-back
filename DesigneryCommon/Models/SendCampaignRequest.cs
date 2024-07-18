using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class SendCampaignRequest
    {
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string? ListId { get; set; } = "e1737e366f"; // ערך קבוע
        public string? FromName { get; set; } = "Designery"; // ערך קבוע
        public string? ReplyTo { get; set; } = "d32193412@gmail.com"; // ערך קבוע
    }
}
