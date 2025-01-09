using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.Sessions
{
    public class SessionRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public SessionRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Session_To_Database()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>() .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) .Options; // Arrange var today = DateTime.Today; using (var context = new AppDbContext(options)) { context.Payments.Add(new Payment { FromUserId = 1, ToUserId = 2, Amount = 100.00m, PaymentDate = today }); context.Payments.Add(new Payment { FromUserId = 2, ToUserId = 3, Amount = 200.00m, PaymentDate = today }); context.Payments.Add(new Payment { FromUserId = 3, ToUserId = 4, Amount = 300.00m, PaymentDate = today.AddDays(-1) }); await context.SaveChangesAsync(); } using (var context = new AppDbContext(options)) { var repository = new PaymentRepository(context); var all = await repository.GetAllAsync(); // Act var payments = await repository.GetPaymentsByDateAsync(today); var numofpayments = payments.Count(); // Assert Assert.Equal(3, all.Count()); Assert.Equal(2, numofpayments); Assert.All(payments, p => Assert.Equal(today, p.PaymentDate.Date)); } } } }
            // Arrange
            var session = new Session
            {
                ServerId = 1,
                StartTime = DateTime.Now
            };

            using (var context = new AppDbContext(options))
            {
                var repository = new SessionRepository(context);

                // Act
                await repository.AddAsync(session);
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var sessions = context.Sessions.ToList();
                Assert.Single(sessions);
                Assert.Equal(1, sessions.First().ServerId);
            }
        }

        [Fact]
        public async Task GetActiveSessionByServerIdAsync_Should_Return_Active_Session()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Sessions.Add(new Session { ServerId = 1, StartTime = DateTime.Now });
                context.Sessions.Add(new Session { ServerId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new SessionRepository(context);

                // Act
                var session = await repository.GetActiveSessionByServerIdAsync(1);

                // Assert
                Assert.NotNull(session);
                Assert.Equal(1, session.ServerId);
                Assert.Null(session.EndTime);
            }
        }

        [Fact]
        public async Task GetSessionsByDateAsync_Should_Return_Sessions_For_Specified_Date()
        {
            // Arrange
            var today = DateTime.Now.Date;

            using (var context = new AppDbContext(_options))
            {
                context.Sessions.Add(new Session { ServerId = 1, StartTime = today.AddHours(9) });
                context.Sessions.Add(new Session { ServerId = 2, StartTime = today.AddHours(10) });
                context.Sessions.Add(new Session { ServerId = 3, StartTime = today.AddDays(-1) });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new SessionRepository(context);

                // Act
                var sessions = await repository.GetSessionsByDateAsync(today);

                // Assert
                Assert.Equal(2, sessions.Count());
                Assert.All(sessions, s => Assert.Equal(today, s.StartTime.Date));
            }
        }
    }
}