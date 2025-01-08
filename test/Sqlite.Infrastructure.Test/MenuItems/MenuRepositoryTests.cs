using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Sqlite.Infrastructure.Test.MenuItems
{
    public class MenuRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public MenuRepositoryTests()
        {
            // Use an in-memory database for testing
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_MenuItem_To_Database()
        {
            // Arrange
            var menuItem = new MenuItem { Name = "Tea", Category = "Beverages", Price = 3.00m };

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var repository = new MenuRepository(context);

                // Act
                await repository.AddAsync(menuItem);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(_options))
            {
                var menuItems = await context.MenuItems.ToListAsync();
                Assert.Single(menuItems);
                Assert.Equal("Tea", menuItems.First().Name);
                Assert.Equal("Beverages", menuItems.First().Category);
                Assert.Equal(3.00m, menuItems.First().Price);
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_MenuItem_From_Database()
        {
            // Arrange
            var menuItem = new MenuItem { Name = "Tea", Category = "Beverages", Price = 3.00m };

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.MenuItems.Add(menuItem);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new MenuRepository(context);
                await repository.DeleteAsync(menuItem);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(_options))
            {
                var menuItems = await context.MenuItems.ToListAsync();
                Assert.Empty(menuItems);
            }
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_MenuItems()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.MenuItems.AddRange(
                    new MenuItem { Name = "Tea", Category = "Beverages", Price = 3.00m },
                    new MenuItem { Name = "Coffee", Category = "Beverages", Price = 5.00m },
                    new MenuItem { Name = "Sandwich", Category = "Food", Price = 7.00m }
                );
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_options))
            {
                var repository = new MenuRepository(context);
                var menuItems = await repository.GetAllAsync();

                // Assert
                Assert.Equal(3, menuItems.Count());
                Assert.Contains(menuItems, item => item.Name == "Tea");
                Assert.Contains(menuItems, item => item.Name == "Coffee");
                Assert.Contains(menuItems, item => item.Name == "Sandwich");
            }
        }

        [Fact]
        public async Task GetMenuByCategoryAsync_Should_Return_MenuItems_For_Specific_Category()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                context.MenuItems.Add(new MenuItem { Name = "Coffee", Category = "Beverages", Price = 5.00m });
                context.MenuItems.Add(new MenuItem { Name = "Cake", Category = "Desserts", Price = 4.00m });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                var repository = new MenuRepository(context);

                // Act
                var items = await repository.GetMenuByCategoryAsync("Beverages");

                // Assert
                Assert.Single(items);
                Assert.Equal("Coffee", items.First().Name);
            }
        }
    }
}