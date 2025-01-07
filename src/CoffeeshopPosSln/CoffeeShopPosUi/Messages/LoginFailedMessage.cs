using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosUi.Core;

namespace CoffeeShopPosUi.Messages
{
    public class LoginFailedMessage : MessageBase
    {
        
        public LoginFailedMessage(string messageType, object payload = null) : base(messageType, payload)
        {
        }
    }
}