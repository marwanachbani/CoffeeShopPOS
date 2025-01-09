using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Services;
using Moq;

namespace App.test.Reports
{
    public class ReportServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

        public ReportServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _sessionRepositoryMock = new Mock<ISessionRepository>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
        }

        [Fact]
        public async Task GetOrderReportAsync_Should_Calculate_Total_Orders_And_Earnings()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Date = DateTime.Now.AddDays(-1), TotalAmount = 100.00m },
                new Order { Date = DateTime.Now, TotalAmount = 200.00m }
            };

            _orderRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            var service = new ReportService(_orderRepositoryMock.Object, _sessionRepositoryMock.Object, _paymentRepositoryMock.Object);

            // Act
            var report = await service.GetOrderReportAsync(DateTime.Now.AddDays(-2), DateTime.Now);

            // Assert
            Assert.Equal(2, report.TotalOrders);
            Assert.Equal(300.00m, report.TotalEarnings);
            Assert.Contains(DateTime.Now.AddDays(-2).ToShortDateString(), report.ReportPeriod);
        }

        [Fact]
        public async Task GetSessionReportsAsync_Should_Aggregate_Sessions_By_Server()
        {
            // Arrange
            var sessions = new List<Session>
            {
                new Session { ServerId = 1, Server = new User { Username = "Server1" }, TotalEarnings = 150.00m },
                new Session { ServerId = 1, Server = new User { Username = "Server1" }, TotalEarnings = 100.00m },
                new Session { ServerId = 2, Server = new User { Username = "Server2" }, TotalEarnings = 200.00m }
            };

            _sessionRepositoryMock.Setup(repo => repo.GetSessionsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(sessions);

            var service = new ReportService(_orderRepositoryMock.Object, _sessionRepositoryMock.Object, _paymentRepositoryMock.Object);

            // Act
            var reports = await service.GetSessionReportsAsync(DateTime.Now.AddDays(-2), DateTime.Now);

            // Assert
            Assert.Equal(2, reports.Count());
            Assert.Contains(reports, r => r.ServerId == 1 && r.TotalEarnings == 250.00m);
            Assert.Contains(reports, r => r.ServerId == 2 && r.TotalEarnings == 200.00m);
        }

        [Fact]
        public async Task GetPaymentReportsAsync_Should_Aggregate_Payments_By_User()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { FromUser = new User { Username = "Server1" }, ToUser = new User { Username = "Manager" }, Amount = 100.00m },
                new Payment { FromUser = new User { Username = "Server2" }, ToUser = new User { Username = "Manager" }, Amount = 200.00m }
            };

            _paymentRepositoryMock.Setup(repo => repo.GetPaymentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(payments);

            var service = new ReportService(_orderRepositoryMock.Object, _sessionRepositoryMock.Object, _paymentRepositoryMock.Object);

            // Act
            var reports = await service.GetPaymentReportsAsync(DateTime.Now.AddDays(-2), DateTime.Now);

            // Assert
            Assert.Equal(2, reports.Count());
            Assert.Contains(reports, r => r.FromUser == "Server1" && r.TotalAmount == 100.00m);
            Assert.Contains(reports, r => r.FromUser == "Server2" && r.TotalAmount == 200.00m);
        }
    }
}