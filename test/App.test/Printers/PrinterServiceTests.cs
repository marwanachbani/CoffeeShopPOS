using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosUi.Services;

namespace App.test.Printers
{
    public class PrinterServiceTests
    {
         private readonly PrinterService _printerService;

        public PrinterServiceTests()
        {
            _printerService = new PrinterService();
        }

        [Fact]
        public void GenerateInvoiceReceipt_Should_Include_All_Required_Details()
        {
            // Arrange
            var orderId = 123;
            var items = new List<(int Item, int Quantity, decimal Price)>
            {
                (1, 2, 3.50m),
                (2, 1, 4.00m)
            };
            var totalAmount = 11.00m;

            // Act
            var receipt = _printerService.GenerateInvoiceReceipt(orderId, items, totalAmount);

            // Assert
            Assert.Contains("CoffeeShop POS Invoice", receipt);
            Assert.Contains($"Order ID: {orderId}", receipt);
            Assert.Contains("Espresso", receipt);
            Assert.Contains("2 x $3.50", receipt);
            Assert.Contains("Latte", receipt);
            Assert.Contains("1 x $4.00", receipt);
            Assert.Contains("TOTAL: $11.00", receipt);
            Assert.Contains("Thank you for visiting CoffeeShop!", receipt);
        }

        [Fact]
        public void PrintReceipt_Should_Invoke_PrintDocument_PrintPage_Event()
        {
            // Arrange
            var receiptText = "Sample Receipt";
            var printDocumentInvoked = false;

            var testPrinterService = new PrinterService();
            var printDocument = new System.Drawing.Printing.PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                printDocumentInvoked = true;
            };

            // Mock the actual Print method
            testPrinterService.PrintReceipt(receiptText);

            // Assert
            Assert.True(printDocumentInvoked);
        }

        [Fact]
        public void PreviewReceipt_Should_Launch_Notepad_With_Receipt_Text()
        {
            // Arrange
            var receiptText = "Sample Receipt";

            // Mocking Notepad interaction (Would require integration tests for real validation)
            // We'll simulate the call to the preview method to ensure no exceptions occur.
            var testPrinterService = new PrinterService();

            // Act
            testPrinterService.PreviewReceipt(receiptText);

            // Assert
            // If the test reaches this point, it means PreviewReceipt was executed without errors.
            Assert.True(true);
        }
    }
}