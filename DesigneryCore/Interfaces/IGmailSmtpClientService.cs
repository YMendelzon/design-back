using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Interfaces
{
    public interface IGmailSmtpClientService

    {
        void SendEmail(string toAddress, string subject, string body, bool isBodyHtml = false);

    }
}




