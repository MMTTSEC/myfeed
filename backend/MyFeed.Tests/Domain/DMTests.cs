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
            Assert.Throws<ArgumentException>(() =>
            new DM(senderUserId: 1, receiverUserId: 1, "Hello")
    );
        }

        [Fact]
        public void CreatingDM_WithEmptyMessage_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new DM(1, 2, ""));

        }
        

    }
}