using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosUi.Core
{
    public class Messenger : IMessenger
    {
        private readonly Dictionary<string, List<Delegate>> _subscribers = new();

    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        var messageType = typeof(TMessage).FullName;
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (_subscribers.ContainsKey(messageType))
        {
            foreach (var callback in _subscribers[messageType])
            {
                ((Action<TMessage>)callback)(message);
            }
        }
    }

    public void Subscribe<TMessage>(Action<TMessage> callback) where TMessage : class
    {
        var messageType = typeof(TMessage).FullName;
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (!_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType] = new List<Delegate>();
        }

        _subscribers[messageType].Add(callback);
    }

    public void Unsubscribe<TMessage>(Action<TMessage> callback) where TMessage : class
    {
        var messageType = typeof(TMessage).FullName;
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType].Remove(callback);
            if (_subscribers[messageType].Count == 0)
            {
                _subscribers.Remove(messageType);
            }
        }
    }

    public void Subscribe(string messageType, Action<MessageBase> callback)
    {
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (!_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType] = new List<Delegate>();
        }

        _subscribers[messageType].Add(callback);
    }

    public void Unsubscribe(string messageType, Action<MessageBase> callback)
    {
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType].Remove(callback);
            if (_subscribers[messageType].Count == 0)
            {
                _subscribers.Remove(messageType);
            }
        }
    }

    public void Send(MessageBase message)
    {
        var messageType = message.GetType().FullName;
        if (messageType == null) throw new ArgumentNullException(nameof(messageType));

        if (_subscribers.ContainsKey(messageType))
        {
            foreach (var callback in _subscribers[messageType])
            {
                ((Action<MessageBase>)callback)(message);
            }
        }
    }
    }
}