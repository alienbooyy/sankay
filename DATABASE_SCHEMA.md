# Database Schema Documentation

## Tables

### Tables
Stores restaurant table information.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| Name | TEXT | Table name (e.g., "Masa 1") |
| Status | INTEGER | 0 = Available, 1 = Occupied |
| Position | INTEGER | Display order position |

### Products
Menu items with pricing.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| Name | TEXT | Product name |
| Price | REAL | Price in TL |
| Category | TEXT | Category (e.g., "Ana Yemek", "İçecek") |

### RawMaterials
Inventory items for recipes.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| Name | TEXT | Material name |
| Unit | TEXT | Unit of measurement (gr, ml, adet) |
| CurrentStock | REAL | Current stock level |
| MinimumStock | REAL | Minimum threshold for warnings |
| CostPerUnit | REAL | Cost per unit in TL |

### Recipes
Links products to their ingredient requirements.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| ProductId | INTEGER | Foreign key to Products |
| RawMaterialId | INTEGER | Foreign key to RawMaterials |
| Quantity | REAL | Amount needed per product unit |

### Orders
Customer orders.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| TableId | INTEGER | Foreign key to Tables |
| OrderDate | TEXT | ISO 8601 datetime |
| TotalAmount | REAL | Total price in TL |
| Status | INTEGER | 0 = Open, 1 = Paid, 2 = Cancelled |
| PaymentMethod | TEXT | Payment type (e.g., "Nakit", "Kredi Kartı") |

### OrderItems
Individual items within orders.

| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key, auto-increment |
| OrderId | INTEGER | Foreign key to Orders |
| ProductId | INTEGER | Foreign key to Products |
| Quantity | INTEGER | Number of units |
| UnitPrice | REAL | Price per unit at time of order |

### Settings
Application configuration.

| Key | Value | Description |
|-----|-------|-------------|
| AdminPassword | 1234 | Admin panel password |
| TabletServerEnabled | false | Enable/disable tablet server |
| TabletServerIP | 192.168.1.35 | IP for tablet connections |
| TabletServerPort | 8080 | Port for tablet server |
| KitchenPrinterIP | 192.168.1.100 | Kitchen printer IP address |
| OvenPrinterIP | 192.168.1.101 | Oven printer IP address |

## Indexes

Recommended indexes for performance:
```sql
CREATE INDEX idx_orders_tableid ON Orders(TableId);
CREATE INDEX idx_orders_status ON Orders(Status);
CREATE INDEX idx_orderitems_orderid ON OrderItems(OrderId);
CREATE INDEX idx_recipes_productid ON Recipes(ProductId);
```

## Business Logic

### Stock Deduction
When an order is paid (Status = 1):
1. Get all recipes for each product in the order
2. Calculate total material usage: Recipe.Quantity × OrderItem.Quantity
3. Deduct from RawMaterials.CurrentStock
4. Check if stock falls below MinimumStock for warnings

### Table Status
- Set to Occupied (1) when order is created
- Set to Available (0) when order is paid or cancelled

### Order Total
Automatically calculated as:
```
SUM(OrderItems.Quantity × OrderItems.UnitPrice)
```

## Sample Queries

### Daily Revenue
```sql
SELECT SUM(TotalAmount) 
FROM Orders 
WHERE date(OrderDate) = date('now') 
AND Status = 1;
```

### Top Selling Products
```sql
SELECT p.Name, SUM(oi.Quantity) as TotalSold
FROM OrderItems oi
JOIN Products p ON oi.ProductId = p.Id
JOIN Orders o ON oi.OrderId = o.Id
WHERE o.Status = 1
GROUP BY p.Name
ORDER BY TotalSold DESC
LIMIT 10;
```

### Low Stock Items
```sql
SELECT Name, CurrentStock, MinimumStock, Unit
FROM RawMaterials
WHERE CurrentStock < MinimumStock;
```
