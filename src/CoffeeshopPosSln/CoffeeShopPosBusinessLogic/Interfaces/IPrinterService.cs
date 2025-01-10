using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface IPrinterService
    {
        string GenerateInvoiceReceipt(int orderId, IEnumerable<(int Item, int Quantity, decimal Price)> items, decimal totalAmount);
        void PrintReceipt(string receiptText);
        void PreviewReceipt(string receiptText);
    }
}