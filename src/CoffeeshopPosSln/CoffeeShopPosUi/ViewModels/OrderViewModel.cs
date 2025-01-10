using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.Views;

namespace CoffeeShopPosUi.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IPrinterService _printerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessenger _messenger;
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public Order CurrentOrder { get; set; } = new Order();

        public ICommand AddOrderCommand { get; }
        public ICommand LoadOrdersCommand { get; }

        public OrderViewModel(IMessenger messenger, IOrderRepository orderRepository,IPrinterService printerService) : base(messenger)
        {
            _messenger = messenger;
            _orderRepository = orderRepository;
            _printerService = printerService;

            AddOrderCommand = new RelayCommand(async (obj) => await AddOrderAsync(CurrentOrder));
            LoadOrdersCommand = new RelayCommand(async (obj) => await LoadOrdersAsync());
        }

        public async Task AddOrderAsync(Order order)
        {
            if (order == null || order.OrderDetails == null || !order.OrderDetails.Any())
            {
                _messenger.Send(new MessageBase("OrderError", "Cannot place an order with no items"));
                return;
            }

            await _orderRepository.AddAsync(order);
        }
        private async Task CompleteOrderAsync()
        {
            // Existing order completion logic...

            // Generate receipt
            var items = CurrentOrder.OrderDetails.Select(d => (d.MenuItemId, d.Quantity, d.Price));
            var receipt = _printerService.GenerateInvoiceReceipt(CurrentOrder.Id, items, CurrentOrder.TotalAmount);

            // Show preview
            var printPreview = new PrintPreviewView
            {
                DataContext = new PrintPreviewViewModel(_messenger, _printerService)
                {
                    ReceiptText = receipt
                }
            };
            printPreview.ShowDialog();
            await Task.CompletedTask;
        }

        private async Task LoadOrdersAsync()
        {
            var todayOrders = await _orderRepository.GetOrdersByDateAsync(DateTime.Now);
            Orders.Clear();

            foreach (var order in todayOrders)
            {
                Orders.Add(order);
            }
        }
    }
}