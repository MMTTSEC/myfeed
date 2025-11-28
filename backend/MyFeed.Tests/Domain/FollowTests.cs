using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class FollowTests
    {
        [Fact]
        public void CreatingFollow_withValidData_Succeeds()
        {
            // Arrange
            int followerUserId = 1;
            int followedUserId = 2;

            // Act
            var follow = new Follow(followerUserId, followedUserId);

            // Assert
            Assert.Equal(followerUserId, follow.FollowerId);
            Assert.Equal(followedUserId, follow.FolloweeId);
            Assert.True((DateTime.UtcNow - follow.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public void CreatingFollow_WithSameFollowerAndFollowee_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Follow(1, 1));
        }
    }
}
