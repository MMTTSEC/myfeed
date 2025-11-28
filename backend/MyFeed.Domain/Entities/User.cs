using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Domain.Entities
{
    public class User
    {
        public string Username { get; }
        public string PasswordHash { get; }
        public User(string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(username));
            }
            Username = username;
            PasswordHash = passwordHash;
            if (Username.Length > 50)
            {
                throw new ArgumentException("Username cannot be longer than 50 characters.", nameof(username));
            }
        }
    }
}
