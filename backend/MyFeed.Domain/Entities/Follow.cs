using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Follow : Entity
    {
        public int FollowerId { get; }
        public int FolloweeId { get; }

        public Follow(int followerId, int followeeId) : base()
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
            if (followerId == followeeId)
            {
                throw new ArgumentException("A user cannot follow themselves.");
            }
        }
    }
}
