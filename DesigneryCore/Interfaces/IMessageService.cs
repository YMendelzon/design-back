using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetAllMessagesAsync();
        Task<MessageDto> GetMessageByIdAsync(int messageId);
        Task<bool> DeleteMessageAsync(int messageId);
    }
}
