using SankayPOS.Models;
using System.Text;

namespace SankayPOS.Utils;

/// <summary>
/// Printer utility for SPENTA thermal printers
/// Currently provides print formatting. Actual printer integration to be added.
/// </summary>
public static class PrinterHelper
{
    public enum PrinterType
    {
        Kitchen,
        Oven
    }
    
    /// <summary>
    /// Formats an order for printing to a thermal printer
    /// </summary>
    public static string FormatOrderForPrint(Order order, string tableName, PrinterType printerType)
    {
        var sb = new StringBuilder();
        
        // Header
        sb.AppendLine("================================");
        sb.AppendLine("     SANKAY RESTAURANT");
        sb.AppendLine($"       {printerType} BÖLÜMÜ");
        sb.AppendLine("================================");
        sb.AppendLine();
        
        // Table and date info
        sb.AppendLine($"MASA: {tableName}");
        sb.AppendLine($"TARİH: {order.OrderDate:dd.MM.yyyy HH:mm}");
        sb.AppendLine($"SİPARİŞ NO: {order.Id}");
        sb.AppendLine();
        sb.AppendLine("--------------------------------");
        
        // Items
        foreach (var item in order.Items)
        {
            sb.AppendLine($"{item.Quantity} x {item.ProductName}");
        }
        
        sb.AppendLine("--------------------------------");
        sb.AppendLine();
        
        // Footer
        sb.AppendLine($"TOPLAM: {order.TotalAmount:C}");
        sb.AppendLine();
        sb.AppendLine("================================");
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Formats a receipt for customer
    /// </summary>
    public static string FormatReceiptForPrint(Order order, string tableName)
    {
        var sb = new StringBuilder();
        
        // Header
        sb.AppendLine("================================");
        sb.AppendLine("     SANKAY RESTAURANT");
        sb.AppendLine("================================");
        sb.AppendLine();
        
        // Table and date info
        sb.AppendLine($"MASA: {tableName}");
        sb.AppendLine($"TARİH: {order.OrderDate:dd.MM.yyyy HH:mm}");
        sb.AppendLine($"FİŞ NO: {order.Id}");
        sb.AppendLine();
        sb.AppendLine("--------------------------------");
        sb.AppendLine("ÜRÜN             ADET    TUTAR");
        sb.AppendLine("--------------------------------");
        
        // Items
        foreach (var item in order.Items)
        {
            var productName = item.ProductName.Length > 16 
                ? item.ProductName.Substring(0, 16) 
                : item.ProductName.PadRight(16);
            var qty = item.Quantity.ToString().PadLeft(4);
            var total = item.TotalPrice.ToString("F2").PadLeft(8);
            sb.AppendLine($"{productName} {qty} {total}");
        }
        
        sb.AppendLine("--------------------------------");
        
        // Totals
        sb.AppendLine($"ARA TOPLAM:           {order.TotalAmount:F2}".PadLeft(32));
        sb.AppendLine($"KDV (%18):            {(order.TotalAmount * 0.18m):F2}".PadLeft(32));
        sb.AppendLine();
        sb.AppendLine($"GENEL TOPLAM:         {(order.TotalAmount * 1.18m):F2}".PadLeft(32));
        sb.AppendLine();
        
        // Payment method
        if (!string.IsNullOrEmpty(order.PaymentMethod))
        {
            sb.AppendLine($"ÖDEME: {order.PaymentMethod}");
            sb.AppendLine();
        }
        
        // Footer
        sb.AppendLine("================================");
        sb.AppendLine("   Bizi tercih ettiğiniz için");
        sb.AppendLine("        teşekkür ederiz!");
        sb.AppendLine("================================");
        sb.AppendLine();
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Sends print job to SPENTA thermal printer
    /// To be implemented with actual printer SDK/drivers
    /// </summary>
    public static void PrintToThermalPrinter(string content, PrinterType printerType)
    {
        // TODO: Integrate with SPENTA printer SDK
        // For now, just output to debug console
        System.Diagnostics.Debug.WriteLine($"=== PRINT TO {printerType} PRINTER ===");
        System.Diagnostics.Debug.WriteLine(content);
        System.Diagnostics.Debug.WriteLine("=== END PRINT ===");
        
        // In production, this would:
        // 1. Connect to the appropriate printer (kitchen or oven) via IP/USB
        // 2. Send ESC/POS commands to format the text
        // 3. Handle printer errors and retry logic
        // 4. Confirm successful print
    }
    
    /// <summary>
    /// Gets the printer IP address from settings
    /// </summary>
    public static string GetPrinterIP(PrinterType printerType)
    {
        var settingKey = printerType switch
        {
            PrinterType.Kitchen => "KitchenPrinterIP",
            PrinterType.Oven => "OvenPrinterIP",
            _ => throw new ArgumentException($"Unknown printer type: {printerType}", nameof(printerType))
        };
        
        var result = Database.DatabaseHelper.ExecuteScalar(
            "SELECT Value FROM Settings WHERE Key = @key",
            new System.Data.SQLite.SQLiteParameter("@key", settingKey));
        
        return result?.ToString() ?? "192.168.1.100";
    }
}
