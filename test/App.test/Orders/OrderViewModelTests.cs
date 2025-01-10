using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace App.test.Orders
{
    public class OrderViewModelTests
    {
        private readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IPrinterService _printerService;
        private readonly Mock<IMessenger> _messengerMock;

        public OrderViewModelTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _messengerMock = new Mock<IMessenger>();
            _printerService = new Mock<IPrinterService>().Object;
        }

       
        [Fact]
        public async Task AddAsync_Should_Add_Order_To_Database()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var repository = new OrderRepository(context);
                var order = new Order { TotalAmount = 100, Date = DateTime.Now };

                // Act
                await repository.AddAsync(order);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(_options))
            {
                var orders = context.Orders.ToList();
                Assert.Single(orders);
                Assert.Equal(100, orders.First().TotalAmount);
            }
        }

        [Fact]
        public async Task AddOrderAsync_Should_Send_OrderError_Message_When_No_OrderDetails()
        {
            var viewModel = new OrderViewModel(_messengerMock.Object, _orderRepositoryMock.Object, _printerService);

            var order = new Order { OrderDetails = null };

            // Act
            await viewModel.AddOrderAsync(order);

            // Assert
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "OrderError" && msg.Payload.ToString().Contains("Cannot place an order with no items"))), Times.Once);
        }

        [Fact]
        public async Task LoadOrdersAsync_Should_Populate_Orders_With_Data_From_Repository()
        {
            // Arrange
            var orders = new[]
            {
                new Order { Id = 1, TotalAmount = 100, Date = DateTime.Now },
                new Order { Id = 2, TotalAmount = 200, Date = DateTime.Now }
            };

            _orderRepositoryMock.Setup(repo => repo.GetOrdersByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(orders);

            var viewModel = new OrderViewModel(_messengerMock.Object, _orderRepositoryMock.Object, _printerService);

            // Act
            viewModel.LoadOrdersCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.Orders.Count);
            Assert.Contains(viewModel.Orders, o => o.Id == 1);
            Assert.Contains(viewModel.Orders, o => o.Id == 2);
        }
    }
}