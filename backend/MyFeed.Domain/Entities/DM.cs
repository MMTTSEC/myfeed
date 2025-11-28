using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class DM
    {
        public int SenderUserId { get; }
        public int ReceiverUserId { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }
        public DM(int senderUserId, int receiverUserId, string message)
        {
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            
            if (senderUserId == receiverUserId)
            {
                throw new ArgumentException("A user cannot send a DM to themselves.");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("DM message cannot be empty.");
            }
            if (message.Length > 1000)
            {
                throw new ArgumentException("DM message cannot be longer than 1000 characters.");
            }
        }

    }
}
