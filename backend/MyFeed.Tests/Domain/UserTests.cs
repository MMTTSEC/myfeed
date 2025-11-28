using System;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class UserTests
    {
        [Fact]
        public void CreatingUser_WithEmptyUsername_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new User("", "somehash"));
            Assert.Throws<ArgumentException>(() => new User("   ", "somehash"));
        }

    }
}
