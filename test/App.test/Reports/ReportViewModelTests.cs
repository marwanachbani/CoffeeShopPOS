using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Reports
{
    public class ReportViewModelTests
    {
        private readonly Mock<IReportService> _reportServiceMock;
        private readonly Mock<IMessenger> _messengerMock;

        public ReportViewModelTests()
        {
            _reportServiceMock = new Mock<IReportService>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task GenerateReportsCommand_Should_Populate_OrderReports()
        {
            // Arrange
            var orderReport = new OrderReport { TotalOrders = 10, TotalEarnings = 500.00m };

            _reportServiceMock.Setup(service => service.GetOrderReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(orderReport);

            var viewModel = new ReportViewModel(_messengerMock.Object, _reportServiceMock.Object);

            // Act
            viewModel.GenerateReportsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Single(viewModel.OrderReports);
            Assert.Equal(10, viewModel.OrderReports.First().TotalOrders);
            Assert.Equal(500.00m, viewModel.OrderReports.First().TotalEarnings);
        }

        [Fact]
        public async Task GenerateReportsCommand_Should_Populate_SessionReports()
        {
            // Arrange
            var sessionReports = new[]
            {
                new SessionReport { ServerId = 1, ServerName = "Server1", TotalEarnings = 300.00m, TotalSessions = 5 },
                new SessionReport { ServerId = 2, ServerName = "Server2", TotalEarnings = 200.00m, TotalSessions = 3 }
            };

            _reportServiceMock.Setup(service => service.GetSessionReportsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(sessionReports);

            var viewModel = new ReportViewModel(_messengerMock.Object, _reportServiceMock.Object);

            // Act
            viewModel.GenerateReportsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.SessionReports.Count);
            Assert.Contains(viewModel.SessionReports, r => r.ServerName == "Server1" && r.TotalEarnings == 300.00m);
            Assert.Contains(viewModel.SessionReports, r => r.ServerName == "Server2" && r.TotalEarnings == 200.00m);
        }

        [Fact]
        public async Task GenerateReportsCommand_Should_Populate_PaymentReports()
        {
            // Arrange
            var paymentReports = new[]
            {
                new PaymentReport { FromUser = "Server1", ToUser = "Manager", TotalAmount = 150.00m },
                new PaymentReport { FromUser = "Server2", ToUser = "Manager", TotalAmount = 250.00m }
            };

            _reportServiceMock.Setup(service => service.GetPaymentReportsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(paymentReports);

            var viewModel = new ReportViewModel(_messengerMock.Object, _reportServiceMock.Object);

            // Act
            viewModel.GenerateReportsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.PaymentReports.Count);
            Assert.Contains(viewModel.PaymentReports, r => r.FromUser == "Server1" && r.TotalAmount == 150.00m);
            Assert.Contains(viewModel.PaymentReports, r => r.FromUser == "Server2" && r.TotalAmount == 250.00m);
        }
    }
}