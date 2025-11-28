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
        public int ReceiverUserId { get; }
        public string Title { get; }
        public string Body { get; }
        public DateTime CreatedAt { get; }
        public Post(int authorUserId, int receiverUserId, string title, string body)
        {
            if (authorUserId == receiverUserId)
            {
                throw new ArgumentException("Author and receiver cannot be the same user.");
            }
            AuthorUserId = authorUserId;
            ReceiverUserId = receiverUserId;
            Title = title;
            Body = body;
            CreatedAt = DateTime.UtcNow;
        }

    }
}
