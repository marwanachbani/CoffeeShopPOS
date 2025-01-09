using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Sessions
{
    public class SessionViewModelTests
    {
         private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public SessionViewModelTests()
        {
            _sessionRepositoryMock = new Mock<ISessionRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task StartSessionCommand_Should_Add_Session_And_Send_Message()
        {
            // Arrange
           var session = new Session { ServerId = 1, StartTime = DateTime.Now };
            _sessionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Session>())).Returns(Task.CompletedTask);

            var viewModel = new SessionViewModel(_messengerMock.Object, _sessionRepositoryMock.Object, _orderRepositoryMock.Object)
            {
                CurrentSession = session
            };

            // Act
             viewModel.StartSessionCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _sessionRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Session>(s => s.ServerId == 1)), Times.Once);
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "SessionStarted" && msg.Payload == session)), Times.Once);
        }

        [Fact]
        public async Task EndSessionCommand_Should_Update_Session_And_Send_Message()
        {
            // Arrange
            var session = new Session { ServerId = 1, StartTime = DateTime.Now };
            _sessionRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Session>())).Returns(Task.CompletedTask);

            var viewModel = new SessionViewModel(_messengerMock.Object, _sessionRepositoryMock.Object, _orderRepositoryMock.Object)
            {
                CurrentSession = session
            };

            // Act
            viewModel.EndSessionCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _sessionRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Session>(s => s.EndTime != null)), Times.Once);
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "SessionEnded" && msg.Payload == session)), Times.Once);
        }

        [Fact]
        public async Task LoadSessionsCommand_Should_Load_Sessions_Into_Collection()
        {
            // Arrange
            var sessions = new[]
            {
                new Session { ServerId = 1, StartTime = DateTime.Now },
                new Session { ServerId = 2, StartTime = DateTime.Now }
            };

            _sessionRepositoryMock.Setup(repo => repo.GetSessionsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(sessions);

            var viewModel = new SessionViewModel(_messengerMock.Object, _sessionRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            viewModel.LoadSessionsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.Sessions.Count);
            Assert.Contains(viewModel.Sessions, s => s.ServerId == 1);
            Assert.Contains(viewModel.Sessions, s => s.ServerId == 2);
        }
    }
}