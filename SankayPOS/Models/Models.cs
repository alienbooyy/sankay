namespace SankayPOS.Models;

public class Table
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TableStatus Status { get; set; }
    public int Position { get; set; }
}

public enum TableStatus
{
    Available = 0,
    Occupied = 1
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class RawMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal CostPerUnit { get; set; }
}

public class Recipe
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int RawMaterialId { get; set; }
    public decimal Quantity { get; set; }
    public string? RawMaterialName { get; set; }
    public string? Unit { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public enum OrderStatus
{
    Open = 0,
    Paid = 1,
    Cancelled = 2
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
