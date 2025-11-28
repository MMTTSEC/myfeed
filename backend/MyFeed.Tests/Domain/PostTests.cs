using System;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class PostTests
    {
        [Fact]
        public void CreatingPost_WithEmptyBody_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Post(authorUserId: 1, title: "Hello", body: "")
            );
        }

        [Fact]
        public void CreatingPost_WithTitleLongerThan100Chars_ThrowsException()
        {
            string longTitle = new string('a', 101);
            Assert.Throws<ArgumentException>(() =>
                new Post(authorUserId: 1, title: longTitle, body: "Body")
            );
        }

        [Fact]
        public void CreatingPost_withValidData_Succeeds()
        {
            var post = new Post(authorUserId: 1, title: "Valid Title", body: "Valid Body");
            Assert.Equal(1, post.AuthorUserId);
            Assert.Equal("Valid Title", post.Title);
            Assert.Equal("Valid Body", post.Body);
            Assert.True((DateTime.UtcNow - post.CreatedAt).TotalSeconds < 5); // CreatedAt is recent
        }

    }
}
