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

        [Fact]
        public void CreatingDM_withValidData_Succeeds()
        {
            var dm = new DM(1, 2, "Hello!");

            Assert.Equal(1, dm.SenderUserId);
            Assert.Equal(2, dm.ReceiverUserId);
            Assert.Equal("Hello!", dm.Message);
            Assert.True((DateTime.UtcNow - dm.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public void CreatingDM_MaxMessageLength_ThrowsException()
        {
            string longMessage = new string('a', 1001); 
            Assert.Throws<ArgumentException>(() => new DM(1, 2, longMessage));

        }


    }
}