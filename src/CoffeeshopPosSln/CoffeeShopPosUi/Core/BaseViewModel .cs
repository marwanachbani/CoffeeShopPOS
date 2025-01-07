using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosUi.Core
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IMessenger Messenger;

        public BaseViewModel(IMessenger messenger)
        {
            Messenger = messenger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SendMessage(string messageType, object payload = null)
        {
            Messenger.Send(new MessageBase(messageType, payload));
        }

        protected void SubscribeMessage(string messageType, Action<MessageBase> callback)
        {
            Messenger.Subscribe(messageType, callback);
        }

        protected void UnsubscribeMessage(string messageType, Action<MessageBase> callback)
        {
            Messenger.Unsubscribe(messageType, callback);
        }
    }
}