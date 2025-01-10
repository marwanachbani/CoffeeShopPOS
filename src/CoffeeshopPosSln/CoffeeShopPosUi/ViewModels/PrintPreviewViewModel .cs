using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosUi.Core;

namespace CoffeeShopPosUi.ViewModels
{
    public class PrintPreviewViewModel : BaseViewModel
    {
        private readonly IPrinterService _printerService;

        public string ReceiptText { get; set; }

        public ICommand PrintCommand { get; }

        public PrintPreviewViewModel(IMessenger messenger, IPrinterService printerService) : base(messenger)
        {
            _printerService = printerService;
            PrintCommand = new RelayCommand(async (param) => await PrintReceiptAsync());
        }
        private Task PrintReceiptAsync(object param = null)
        {
            _printerService.PrintReceipt(ReceiptText);
            return Task.CompletedTask;
        }
    }
}