using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;

namespace CoffeeShopPosUi.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly IMenuRepository _menuRepository;

        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();
        public MenuItem CurrentMenuItem { get; set; } = new MenuItem();

        public ICommand AddMenuItemCommand { get; }
        public ICommand DeleteMenuItemCommand { get; }
        public ICommand LoadMenuCommand { get; }

        public MenuViewModel(Messenger messenger, IMenuRepository menuRepository) : base(messenger)
        {
            _menuRepository = menuRepository;

            AddMenuItemCommand = new RelayCommand(async (obj) => await AddMenuItemAsync());
            DeleteMenuItemCommand = new RelayCommand(async (obj) => await DeleteMenuItemAsync());
            LoadMenuCommand = new RelayCommand(async (obj) => await LoadMenuAsync());
        }

        private async Task AddMenuItemAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentMenuItem.Name) || CurrentMenuItem.Price <= 0)
            {
                SendMessage("MenuError", "Invalid menu item details.");
                return;
            }

            await _menuRepository.AddAsync(CurrentMenuItem);
            MenuItems.Add(CurrentMenuItem);

            CurrentMenuItem = new MenuItem();
            OnPropertyChanged(nameof(CurrentMenuItem));
        }

        private async Task DeleteMenuItemAsync()
        {
            if (CurrentMenuItem == null || CurrentMenuItem.Id == 0)
            {
                SendMessage("MenuError", "No menu item selected for deletion.");
                return;
            }

            await _menuRepository.DeleteAsync(CurrentMenuItem);
            MenuItems.Remove(CurrentMenuItem);

            CurrentMenuItem = new MenuItem();
            OnPropertyChanged(nameof(CurrentMenuItem));
        }

        private async Task LoadMenuAsync()
        {
            var menuItems = await _menuRepository.GetAllAsync();
            MenuItems.Clear();

            foreach (var item in menuItems)
            {
                MenuItems.Add(item);
            }
        }
    }
}