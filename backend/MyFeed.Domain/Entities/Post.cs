using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Post
    {
        public int AuthorUserId { get; }
        public string Title { get; }
        public string Body { get; }
        public DateTime CreatedAt { get; }
        public Post(int authorUserId, string title, string body)
        {
            AuthorUserId = authorUserId;
            Title = title;
            Body = body;
            CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("Post body cannot be empty.");
            }
            if (title.Length > 100)
            {
                throw new ArgumentException("Post title cannot be longer than 100 characters.");
            }

        }

    }
}
