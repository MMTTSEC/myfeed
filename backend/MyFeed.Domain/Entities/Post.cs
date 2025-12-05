using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class Post : Entity
    {
        public int AuthorUserId { get; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        
        public Post(int authorUserId, string title, string body) : base()
        {
            AuthorUserId = authorUserId;
            Title = title;
            Body = body;

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Post title cannot be empty.", nameof(title));
            }
            if (title.Length > 100)
            {
                throw new ArgumentException("Post title cannot be longer than 100 characters.", nameof(title));
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("Post body cannot be empty.", nameof(body));
            }

        }

        public void Update(string title, string body)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Post title cannot be empty.", nameof(title));
            if (title.Length > 100)
                throw new ArgumentException("Post title cannot be longer than 100 characters.", nameof(title));
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Post body cannot be empty.", nameof(body));

            Title = title;
            Body = body;
        }

    }
}
