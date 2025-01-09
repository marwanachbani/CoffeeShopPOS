using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.Payments
{
    public class PaymentRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public PaymentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Payment_To_Database()
        {
            // Arrange
            var payment = new Payment
            {
                FromUserId = 1,
                ToUserId = 2,
                Amount = 100.00m,
                PaymentDate = DateTime.Now
            };

            using (var context = new AppDbContext(_options))
            {
                var repository = new PaymentRepository(context);

                // Act
                await repository.AddAsync(payment);
            }

            // Assert
            using (var context = new AppDbContext(_options))
            {
                var payments = context.Payments.ToList();
                Assert.Single(payments);
                Assert.Equal(100.00m, payments.First().Amount);
            }
        }

        [Fact]
        public async Task GetPaymentsByDateAsync_Should_Return_Payments_For_Specified_Date()
        {
            // Arrange
            var today = DateTime.Today;

            using (var context = new AppDbContext(_options))
            {
                context.Payments.Add(new Payment { FromUserId = 1, ToUserId = 2, Amount = 100.00m, PaymentDate = today });
                context.Payments.Add(new Payment { FromUserId = 2, ToUserId = 3, Amount = 200.00m, PaymentDate = today });
                context.Payments.Add(new Payment { FromUserId = 3, ToUserId = 4, Amount = 300.00m, PaymentDate = today.AddDays(-1) });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PaymentRepository(context);
                var all = await repository.GetAllAsync();
                // Act
                var payments = await repository.GetPaymentsByDateAsync(today);
                var numofpayments = payments.Count();
                // Assert
                Assert.Equal(3, all.Count());
                Assert.Equal(2, numofpayments);
                Assert.All(payments, p => Assert.Equal(today, p.PaymentDate.Date));
            }
        }

        [Fact]
        public async Task GetTotalPaymentsToUserByDateAsync_Should_Return_Total_Payments_To_User_For_Specified_Date()
        {
            // Arrange
            var today = DateTime.Now.Date;

            using (var context = new AppDbContext(_options))
            {
                context.Payments.Add(new Payment { FromUserId = 1, ToUserId = 2, Amount = 100.00m, PaymentDate = today });
                context.Payments.Add(new Payment { FromUserId = 3, ToUserId = 2, Amount = 200.00m, PaymentDate = today });
                context.Payments.Add(new Payment { FromUserId = 4, ToUserId = 3, Amount = 300.00m, PaymentDate = today });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new PaymentRepository(context);

                // Act
                var total = await repository.GetTotalPaymentsToUserByDateAsync(2, today);

                // Assert
                Assert.Equal(300.00m, total);
            }
        }
    }
}