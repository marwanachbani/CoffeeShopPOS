using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosUi.Core
{
    public class MessageBase
    {
        public string MessageType { get; set; }
        public object Payload { get; set; }

        public MessageBase(string messageType, object payload = null)
        {
            MessageType = messageType;
            Payload = payload;
        }
    }
}