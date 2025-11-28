using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Follow
    {
        public int FollowerId { get; }
        public int FolloweeId { get; }
        public DateTime CreatedAt { get; }

        public Follow(int followerId, int followeeId)
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
            CreatedAt = DateTime.UtcNow;
            if (followerId == followeeId)
            {
                throw new ArgumentException("A user cannot follow themselves.");
            }
        }
    }
}
