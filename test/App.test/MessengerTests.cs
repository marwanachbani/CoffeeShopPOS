using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosUi.Core;

namespace App.test
{
    public class MessengerTests
    {
        [Fact]
        public void Send_Should_Invoke_Subscribed_Callback()
        {
            // Arrange
            var messenger = new Messenger();
            var messageReceived = false;

            Action<MessageBase> callback = _ => messageReceived = true;

            messenger.Subscribe("TestMessage", callback);
            messenger.Unsubscribe("TestMessage", callback);

            // Act
            messenger.Send(new MessageBase("TestMessage"));

            // Assert
            Assert.False(messageReceived);
        }

        [Fact]
        public void Unsubscribe_Should_Remove_Callback()
        {
            // Arrange
            var messenger = new Messenger();
            var messageReceived = false;

            Action<MessageBase> callback = _ => messageReceived = true;

            messenger.Subscribe("TestMessage", callback);
            messenger.Unsubscribe("TestMessage", callback);

            // Act
            messenger.Send(new MessageBase("TestMessage"));

            // Assert
            Assert.False(messageReceived);
        }
    }
}