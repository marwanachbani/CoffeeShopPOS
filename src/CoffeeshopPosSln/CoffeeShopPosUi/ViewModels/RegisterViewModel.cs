using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using System.Windows.Input;

namespace CoffeeShopPosUi.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel(IMessenger messenger, IUserRepository userRepository) : base(messenger)
        {
            _userRepository = userRepository;

            RegisterCommand = new RelayCommand(async (param) => await RegisterAsync());
        }

        private async Task RegisterAsync()
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(Username))
                    throw new Exception("Username cannot be empty.");

                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                    throw new Exception("Password must be at least 6 characters long.");

                if (string.IsNullOrWhiteSpace(Role))
                    throw new Exception("Role must be selected.");

                // Check for unique username
                if (!await _userRepository.IsUsernameUniqueAsync(Username))
                    throw new Exception("Username is already taken.");

                // Create and save user
                var user = new User
                {
                    Username = Username,
                    PasswordHash = Password, // Replace with proper hashing in production
                    Role = Role
                };
                await _userRepository.AddAsync(user);

                // Notify success
                SendMessage("UserRegistered", user);
            }
            catch (Exception ex)
            {
                // Notify failure
                SendMessage("RegistrationFailed", ex.Message);
            }
        }
    }
}
