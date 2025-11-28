using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Domain
{
    public class DMTests
    {
        [Fact]
        public void CreatingDM_WithSameSenderAndReceiver_ThrowsException()
        {
            int userId = 1;
            string message = "Hello";

            Assert.Throws<ArgumentException>(() => new DM(userId, userId, message));
        }
        
    }
}