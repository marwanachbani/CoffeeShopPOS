using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Users
{
    public class RegisterViewModelTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public RegisterViewModelTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task RegisterAsync_Should_Send_UserRegistered_Message_On_Success()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.IsUsernameUniqueAsync("newuser")).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var viewModel = new RegisterViewModel(_messengerMock.Object, _userRepositoryMock.Object)
            {
                Username = "newuser",
                Password = "password123",
                Role = "Cashier"
            };

            // Act
            viewModel.RegisterCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "UserRegistered" && msg.Payload is User)), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Send_RegistrationFailed_Message_On_Invalid_Data()
        {
            // Arrange
            var viewModel = new RegisterViewModel(_messengerMock.Object, _userRepositoryMock.Object)
            {
                Username = " ",
                Password = "short",
                Role = "Cashier"
            };

            // Act
            viewModel.RegisterCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "RegistrationFailed" && msg.Payload.ToString().Contains("Username cannot be empty"))), Times.Once);
        }
    }
}
