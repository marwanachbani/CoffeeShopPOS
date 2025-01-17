﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.Users
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task GetByUsernameAsync_Should_Return_User_If_Exists()
        {
            using var context = new AppDbContext(_options);
            context.Users.Add(new User { Username = "admin", PasswordHash = "admin123" });
            context.SaveChanges();

            var repository = new UserRepository(context);

            var user = await repository.GetByUsernameAsync("admin");

            Assert.NotNull(user);
            Assert.Equal("admin", user.Username);
        }

        [Fact]
        public async Task GetByUsernameAsync_Should_Return_Null_If_User_Does_Not_Exist()
        {
            using var context = new AppDbContext(_options);
            var repository = new UserRepository(context);

            var user = await repository.GetByUsernameAsync("nonexistent");

            Assert.Null(user);
        }
        [Fact]
        public async Task IsUsernameUniqueAsync_Should_Return_True_When_Username_Is_Unique()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Users.Add(new User { Username = "existinguser", PasswordHash = "password", Role = "Admin" });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var userRepository = new UserRepository(context);

                // Act
                var result = await userRepository.IsUsernameUniqueAsync("uniqueuser");

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task IsUsernameUniqueAsync_Should_Return_False_When_Username_Is_Not_Unique()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Users.Add(new User { Username = "existinguser", PasswordHash = "password", Role = "Admin" });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var userRepository = new UserRepository(context);

                // Act
                var result = await userRepository.IsUsernameUniqueAsync("existinguser");

                // Assert
                Assert.False(result);
            }
        }
    }
}
