using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.Messages;

namespace CoffeeShopPosUi.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessenger _messenger;

        public LoginViewModel(IMessenger messenger, IUserRepository userRepository) : base(messenger)
        {
            _messenger = messenger;
            _userRepository = userRepository;

            // Define the login command
            LoginCommand = new RelayCommand(async (param) => await LoginAsync());
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public async Task LoginAsync()

    {

        var user = await _userRepository.GetByUsernameAsync(Username);

        if (user != null && user.PasswordHash == Password)

        {

            // Login successful

        }

        else

        {

            _messenger.Send(new LoginFailedMessage("logged"));

        }

    }


    }
}