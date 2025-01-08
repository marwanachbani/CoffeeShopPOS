using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.MenuItems
{
    public class MenuViewModelTests
    {
        private readonly Mock<IMenuRepository> _menuRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public MenuViewModelTests()
        {
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task AddMenuItemCommand_Should_Add_MenuItem_And_Reset_CurrentMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Name = "Coffee", Category = "Beverages", Price = 5.00m };
            _menuRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            var viewModel = new MenuViewModel(_messengerMock.Object, _menuRepositoryMock.Object)
            {
                CurrentMenuItem = menuItem
            };

            // Act
            viewModel.AddMenuItemCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _menuRepositoryMock.Verify(repo => repo.AddAsync(It.Is<MenuItem>(m => m.Name == "Coffee")), Times.Once);
            Assert.NotNull(viewModel.CurrentMenuItem);
            Assert.Null(viewModel.CurrentMenuItem.Name);
        }

        [Fact]
        public async Task DeleteMenuItemCommand_Should_Remove_MenuItem_From_MenuItems_Collection()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Coffee", Category = "Beverages", Price = 5.00m };
            var viewModel = new MenuViewModel(_messengerMock.Object, _menuRepositoryMock.Object)
            {
                CurrentMenuItem = menuItem,
                MenuItems = new ObservableCollection<MenuItem> { menuItem }
            };

            _menuRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            // Act
            viewModel.DeleteMenuItemCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _menuRepositoryMock.Verify(repo => repo.DeleteAsync(It.Is<MenuItem>(m => m.Id == 1)), Times.Once);
            Assert.Empty(viewModel.MenuItems);
        }

        [Fact]
        public async Task LoadMenuCommand_Should_Load_All_MenuItems_Into_Collection()
        {
            // Arrange
            var menuItems = new[]
            {
                new MenuItem { Name = "Coffee", Category = "Beverages", Price = 5.00m },
                new MenuItem { Name = "Cake", Category = "Desserts", Price = 4.00m }
            };

            _menuRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(menuItems);

            var viewModel = new MenuViewModel(_messengerMock.Object, _menuRepositoryMock.Object);

            // Act
            viewModel.LoadMenuCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.MenuItems.Count);
            Assert.Contains(viewModel.MenuItems, m => m.Name == "Coffee");
            Assert.Contains(viewModel.MenuItems, m => m.Name == "Cake");
        }
    }
}