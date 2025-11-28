using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Like
    {   
        public int UserId { get; }
        public int PostId { get; }
        public DateTime CreatedAt { get; }
        public Like(int userId, int postId)
        {
            UserId = userId;
            PostId = postId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
