using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Like : Entity
    {   
        public int UserId { get; }
        public int PostId { get; }
        
        public Like(int userId, int postId) : base()
        {
            UserId = userId;
            PostId = postId;
        }
    }
}
