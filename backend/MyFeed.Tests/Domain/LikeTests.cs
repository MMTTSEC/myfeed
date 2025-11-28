using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class LikeTests
    {
        [Fact]
        public void CreatingLike_withValidData_Succeeds()
        {
            var like = new Like(userId: 1, postId: 2);
            
            Assert.Equal(1, like.UserId);
            Assert.Equal(2, like.PostId);
            Assert.True((DateTime.UtcNow - like.CreatedAt).TotalSeconds < 5);


        }

    }
}
