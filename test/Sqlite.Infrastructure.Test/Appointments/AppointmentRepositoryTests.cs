using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.Appointments
{
    public class AppointmentRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public AppointmentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Appointment_To_Database()
        {
            // Arrange
            var appointment = new Appointment { UserId = 1, Role = "Server", AppointmentDate = DateTime.Now.Date };

            using (var context = new AppDbContext(_options))
            {
                var repository = new AppointmentRepository(context);

                // Act
                await repository.AddAsync(appointment);
            }

            // Assert
            using (var context = new AppDbContext(_options))
            {
                var appointments = context.Appointments.ToList();
                Assert.Single(appointments);
                Assert.Equal("Server", appointments.First().Role);
            }
        }

        [Fact]
        public async Task GetAppointmentsByDateAsync_Should_Return_Appointments_For_Specified_Date()
        {
            // Arrange
            var today = DateTime.Today;
            var user = new User { Username = "testuser", PasswordHash = "password", Role = "User" };
            var appointment = new Appointment { AppointmentDate = today, User = user };
            

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Appointments.Add(appointment);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_options))
            {
                var repository = new AppointmentRepository(context);
                var appointments = await repository.GetAppointmentsByDateAsync(today);

                // Assert
                Assert.Single(appointments);
                Assert.Equal(today, appointments.First().AppointmentDate);
            }
        }
    }
}