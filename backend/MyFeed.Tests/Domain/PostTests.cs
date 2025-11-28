using System;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class PostTests
    {
        [Fact]
        public void CreatingPost_WithAuthorEqualsReceiver_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Post(authorUserId: 1, receiverUserId: 1, "Hello", "Body")
            );
        }   

        [Fact]
        public void CreatingPost_WithEmptyBody_ThrowsNoException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Post(authorUserId: 1, receiverUserId: 2, "Hello", "")
            );
        }

    }
}
