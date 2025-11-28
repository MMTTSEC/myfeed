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

        [Fact]
        public void CreatingUser_WithUsernameLongerThan50Chars_ThrowsException()
        {
            string longUsername = new string('a', 51);
            Assert.Throws<ArgumentException>(() => new User(longUsername, "somehash"));
        }

        [Fact]
        public void CreatingUser_WithEmptyOrNullPasswordHash_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new User("validuser", ""));
            Assert.Throws<ArgumentException>(() => new User("validuser", null!));
        }

    }
}
