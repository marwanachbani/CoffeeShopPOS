using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using CoffeeShopPosBusinessLogic.Interfaces;
using System.Diagnostics;

namespace CoffeeShopPosUi.Services
{
    public class PrinterService : IPrinterService
    {
        public string GenerateInvoiceReceipt(int orderId, IEnumerable<(int Item, int Quantity, decimal Price)> items, decimal totalAmount)
        {
            var receipt = $"CoffeeShop POS Invoice\nOrder ID: {orderId}\n";
            receipt += "----------------------------------\n";

            foreach (var item in items)
            {
                receipt += $"{item.Item,-20} {item.Quantity} x {item.Price:C}\n";
            }

            receipt += "----------------------------------\n";
            receipt += $"TOTAL: {totalAmount:C}\n";
            receipt += "----------------------------------\n";
            receipt += $"Thank you for visiting CoffeeShop!\n";

            return receipt;
        }

        public void PrintReceipt(string receiptText)
        {
            var printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                e.Graphics.DrawString(receiptText, new Font("Courier New", 10), Brushes.Black, new PointF(10, 10));
            };
            printDocument.Print();
        }

        public void PreviewReceipt(string receiptText)
        {
            var previewProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = $"/p",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            previewProcess.Start();

            using (var writer = previewProcess.StandardInput)
            {
                writer.Write(receiptText);
            }
        }

        
    }
}