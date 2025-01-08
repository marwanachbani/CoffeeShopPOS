using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.Orders
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public OrderRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
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
        public async Task GetOrdersByDateAsync_Should_Return_Orders_For_Specific_Date()
        {
            // Arrange
            var today = DateTime.Now.Date;
            using (var context = new AppDbContext(_options))
            {
                context.Orders.Add(new Order { Date = today, TotalAmount = 100 });
                context.Orders.Add(new Order { Date = today, TotalAmount = 200 });
                context.Orders.Add(new Order { Date = today.AddDays(-1), TotalAmount = 150 });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new OrderRepository(context);

                // Act
                var orders = await repository.GetOrdersByDateAsync(today);

                // Assert
                Assert.Equal(2, orders.Count());
                Assert.All(orders, order => Assert.Equal(today, order.Date.Date));
            }
        }
    }
}