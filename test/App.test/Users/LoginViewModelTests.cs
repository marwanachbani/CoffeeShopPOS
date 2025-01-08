using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.Messages;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Users
{
    public class LoginViewModelTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public LoginViewModelTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public void LoginCommand_Should_Execute_LoginAsync()
        {
            // Arrange
            var viewModel = new LoginViewModel(_messengerMock.Object, _userRepositoryMock.Object)
            {
                Username = "testuser",
                Password = "testpassword"
            };

            // Act
            viewModel.LoginCommand.Execute(null);

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync("testuser"), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_Should_Send_LoginFailed_Message_On_Invalid_Credentials()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync("admin")).ReturnsAsync((User)null);
            var viewModel = new LoginViewModel(_messengerMock.Object, _userRepositoryMock.Object)
            {
                Username = "admin",
                Password = "wrongpassword"
            };

            // Act
            await viewModel.LoginAsync();

            // Assert
            _messengerMock.Verify(m => m.Send(It.IsAny<LoginFailedMessage>()), Times.Once);
        }
    }
}
