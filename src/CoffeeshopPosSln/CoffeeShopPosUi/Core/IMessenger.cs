using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopPosUi.Core
{
    public interface IMessenger
    {
        void Send<TMessage>(TMessage message) where TMessage : class;
        void Subscribe(string messageType, Action<MessageBase> callback);
        void Unsubscribe(string messageType, Action<MessageBase> callback);
    }
}
