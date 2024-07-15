using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class MessageDto
    {
        public int MessageId { get; set; }
        public string Message { get; set; }
        public int DataEntryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
