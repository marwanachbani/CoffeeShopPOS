using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Payments
{
    public class PaymentViewModelTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public PaymentViewModelTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _sessionRepositoryMock = new Mock<ISessionRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task ProcessServerToManagerCommand_Should_Add_Payments_For_All_Sessions()
        {
            // Arrange
            var sessions = new[]
            {
                new Session { ServerId = 1, TotalEarnings = 100.00m, EndTime = DateTime.Now },
                new Session { ServerId = 2, TotalEarnings = 200.00m, EndTime = DateTime.Now }
            };

            _sessionRepositoryMock.Setup(repo => repo.GetSessionsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(sessions);

            var viewModel = new PaymentViewModel(_messengerMock.Object, _paymentRepositoryMock.Object, _sessionRepositoryMock.Object)
            {
                CurrentPayment = new Payment { ToUserId = 3 } // Manager ID
            };

            // Act
            viewModel.ProcessServerToManagerCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _paymentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Payment>()), Times.Exactly(2));
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "ServerToManagerProcessed")), Times.Once);
        }

        [Fact]
        public async Task ProcessManagerToAdminCommand_Should_Add_Consolidated_Payment_To_Admin()
        {
            // Arrange
            _paymentRepositoryMock.Setup(repo => repo.GetTotalPaymentsToUserByDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(300.00m);

            var viewModel = new PaymentViewModel(_messengerMock.Object, _paymentRepositoryMock.Object, _sessionRepositoryMock.Object)
            {
                CurrentPayment = new Payment { FromUserId = 3, ToUserId = 4 } // Manager ID -> Admin ID
            };

            // Act
            viewModel.ProcessManagerToAdminCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _paymentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Payment>(p => p.Amount == 300.00m)), Times.Once);
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "ManagerToAdminProcessed")), Times.Once);
        }

        [Fact]
        public async Task LoadPaymentsCommand_Should_Load_Today_Payments()
        {
            // Arrange
            var payments = new[]
            {
                new Payment { FromUserId = 1, ToUserId = 2, Amount = 100.00m, PaymentDate = DateTime.Now },
                new Payment { FromUserId = 2, ToUserId = 3, Amount = 200.00m, PaymentDate = DateTime.Now }
            };

            _paymentRepositoryMock.Setup(repo => repo.GetPaymentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(payments);

            var viewModel = new PaymentViewModel(_messengerMock.Object, _paymentRepositoryMock.Object, _sessionRepositoryMock.Object);

            // Act
            viewModel.LoadPaymentsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.Payments.Count);
            Assert.Contains(viewModel.Payments, p => p.Amount == 100.00m);
            Assert.Contains(viewModel.Payments, p => p.Amount == 200.00m);
        }
    }
}