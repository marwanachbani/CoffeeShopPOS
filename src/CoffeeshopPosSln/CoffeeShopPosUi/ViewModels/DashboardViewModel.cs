using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;

namespace CoffeeShopPosUi.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private string _loggedInUser;
        public string LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser));
            }
        }

        public DashboardViewModel(Messenger messenger) : base(messenger)
        {
            SubscribeMessage("UserLoggedIn", OnUserLoggedIn);
        }

        private void OnUserLoggedIn(MessageBase message)
        {
            if (message.Payload is User user)
            {
                LoggedInUser = $"Welcome, {user.Username}!";
            }
        }
    }

}
