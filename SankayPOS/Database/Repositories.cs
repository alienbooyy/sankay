using System.Data;
using System.Data.SQLite;
using SankayPOS.Models;

namespace SankayPOS.Database;

public class TableRepository
{
    public List<Table> GetAllTables()
    {
        var tables = new List<Table>();
        var dt = DatabaseHelper.ExecuteQuery("SELECT Id, Name, Status, Position FROM Tables ORDER BY Position");
        
        foreach (DataRow row in dt.Rows)
        {
            tables.Add(new Table
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString() ?? "",
                Status = (TableStatus)Convert.ToInt32(row["Status"]),
                Position = Convert.ToInt32(row["Position"])
            });
        }
        
        return tables;
    }
    
    public void AddTable(string name, int position)
    {
        DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO Tables (Name, Status, Position) VALUES (@name, 0, @position)",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@position", position));
    }
    
    public void UpdateTable(int id, string name)
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE Tables SET Name = @name WHERE Id = @id",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@id", id));
    }
    
    public void DeleteTable(int id)
    {
        DatabaseHelper.ExecuteNonQuery(
            "DELETE FROM Tables WHERE Id = @id",
            new SQLiteParameter("@id", id));
    }
    
    public void UpdateTableStatus(int id, TableStatus status)
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE Tables SET Status = @status WHERE Id = @id",
            new SQLiteParameter("@status", (int)status),
            new SQLiteParameter("@id", id));
    }
}

public class ProductRepository
{
    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        var dt = DatabaseHelper.ExecuteQuery("SELECT Id, Name, Price, Category FROM Products ORDER BY Name");
        
        foreach (DataRow row in dt.Rows)
        {
            products.Add(new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString() ?? "",
                Price = Convert.ToDecimal(row["Price"]),
                Category = row["Category"]?.ToString() ?? ""
            });
        }
        
        return products;
    }
    
    public void AddProduct(string name, decimal price, string category)
    {
        DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO Products (Name, Price, Category) VALUES (@name, @price, @category)",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@price", price),
            new SQLiteParameter("@category", category));
    }
    
    public void UpdateProduct(int id, string name, decimal price, string category)
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE Products SET Name = @name, Price = @price, Category = @category WHERE Id = @id",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@price", price),
            new SQLiteParameter("@category", category),
            new SQLiteParameter("@id", id));
    }
    
    public void DeleteProduct(int id)
    {
        DatabaseHelper.ExecuteNonQuery(
            "DELETE FROM Products WHERE Id = @id",
            new SQLiteParameter("@id", id));
    }
}

public class RawMaterialRepository
{
    public List<RawMaterial> GetAllRawMaterials()
    {
        var materials = new List<RawMaterial>();
        var dt = DatabaseHelper.ExecuteQuery("SELECT Id, Name, Unit, CurrentStock, MinimumStock, CostPerUnit FROM RawMaterials ORDER BY Name");
        
        foreach (DataRow row in dt.Rows)
        {
            materials.Add(new RawMaterial
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString() ?? "",
                Unit = row["Unit"].ToString() ?? "",
                CurrentStock = Convert.ToDecimal(row["CurrentStock"]),
                MinimumStock = Convert.ToDecimal(row["MinimumStock"]),
                CostPerUnit = Convert.ToDecimal(row["CostPerUnit"])
            });
        }
        
        return materials;
    }
    
    public void AddRawMaterial(string name, string unit, decimal currentStock, decimal minimumStock, decimal costPerUnit)
    {
        DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO RawMaterials (Name, Unit, CurrentStock, MinimumStock, CostPerUnit) VALUES (@name, @unit, @currentStock, @minimumStock, @costPerUnit)",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@unit", unit),
            new SQLiteParameter("@currentStock", currentStock),
            new SQLiteParameter("@minimumStock", minimumStock),
            new SQLiteParameter("@costPerUnit", costPerUnit));
    }
    
    public void UpdateRawMaterial(int id, string name, string unit, decimal currentStock, decimal minimumStock, decimal costPerUnit)
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE RawMaterials SET Name = @name, Unit = @unit, CurrentStock = @currentStock, MinimumStock = @minimumStock, CostPerUnit = @costPerUnit WHERE Id = @id",
            new SQLiteParameter("@name", name),
            new SQLiteParameter("@unit", unit),
            new SQLiteParameter("@currentStock", currentStock),
            new SQLiteParameter("@minimumStock", minimumStock),
            new SQLiteParameter("@costPerUnit", costPerUnit),
            new SQLiteParameter("@id", id));
    }
    
    public void UpdateStock(int id, decimal newStock)
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE RawMaterials SET CurrentStock = @stock WHERE Id = @id",
            new SQLiteParameter("@stock", newStock),
            new SQLiteParameter("@id", id));
    }
    
    public void DeleteRawMaterial(int id)
    {
        DatabaseHelper.ExecuteNonQuery(
            "DELETE FROM RawMaterials WHERE Id = @id",
            new SQLiteParameter("@id", id));
    }
}

public class RecipeRepository
{
    public List<Recipe> GetRecipesByProduct(int productId)
    {
        var recipes = new List<Recipe>();
        var dt = DatabaseHelper.ExecuteQuery(
            @"SELECT r.Id, r.ProductId, r.RawMaterialId, r.Quantity, rm.Name as RawMaterialName, rm.Unit 
              FROM Recipes r 
              JOIN RawMaterials rm ON r.RawMaterialId = rm.Id 
              WHERE r.ProductId = @productId",
            new SQLiteParameter("@productId", productId));
        
        foreach (DataRow row in dt.Rows)
        {
            recipes.Add(new Recipe
            {
                Id = Convert.ToInt32(row["Id"]),
                ProductId = Convert.ToInt32(row["ProductId"]),
                RawMaterialId = Convert.ToInt32(row["RawMaterialId"]),
                Quantity = Convert.ToDecimal(row["Quantity"]),
                RawMaterialName = row["RawMaterialName"].ToString(),
                Unit = row["Unit"].ToString()
            });
        }
        
        return recipes;
    }
    
    public void AddRecipe(int productId, int rawMaterialId, decimal quantity)
    {
        DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO Recipes (ProductId, RawMaterialId, Quantity) VALUES (@productId, @rawMaterialId, @quantity)",
            new SQLiteParameter("@productId", productId),
            new SQLiteParameter("@rawMaterialId", rawMaterialId),
            new SQLiteParameter("@quantity", quantity));
    }
    
    public void DeleteRecipe(int id)
    {
        DatabaseHelper.ExecuteNonQuery(
            "DELETE FROM Recipes WHERE Id = @id",
            new SQLiteParameter("@id", id));
    }
    
    public void DeductStockForProduct(int productId, int quantity)
    {
        var recipes = GetRecipesByProduct(productId);
        foreach (var recipe in recipes)
        {
            var totalDeduction = recipe.Quantity * quantity;
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE RawMaterials SET CurrentStock = CurrentStock - @deduction WHERE Id = @id",
                new SQLiteParameter("@deduction", totalDeduction),
                new SQLiteParameter("@id", recipe.RawMaterialId));
        }
    }
}

public class OrderRepository
{
    public int CreateOrder(int tableId)
    {
        DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO Orders (TableId, OrderDate, TotalAmount, Status) VALUES (@tableId, @orderDate, 0, 0)",
            new SQLiteParameter("@tableId", tableId),
            new SQLiteParameter("@orderDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        
        var result = DatabaseHelper.ExecuteScalar("SELECT last_insert_rowid()");
        return Convert.ToInt32(result);
    }
    
    public Order? GetOpenOrderByTable(int tableId)
    {
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT Id, TableId, OrderDate, TotalAmount, Status, PaymentMethod FROM Orders WHERE TableId = @tableId AND Status = 0 ORDER BY Id DESC LIMIT 1",
            new SQLiteParameter("@tableId", tableId));
        
        if (dt.Rows.Count == 0)
            return null;
        
        var row = dt.Rows[0];
        var order = new Order
        {
            Id = Convert.ToInt32(row["Id"]),
            TableId = Convert.ToInt32(row["TableId"]),
            OrderDate = DateTime.Parse(row["OrderDate"].ToString()!),
            TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
            Status = (OrderStatus)Convert.ToInt32(row["Status"]),
            PaymentMethod = row["PaymentMethod"]?.ToString() ?? ""
        };
        
        order.Items = GetOrderItems(order.Id);
        return order;
    }
    
    public List<OrderItem> GetOrderItems(int orderId)
    {
        var items = new List<OrderItem>();
        var dt = DatabaseHelper.ExecuteQuery(
            @"SELECT oi.Id, oi.OrderId, oi.ProductId, p.Name as ProductName, oi.Quantity, oi.UnitPrice 
              FROM OrderItems oi 
              JOIN Products p ON oi.ProductId = p.Id 
              WHERE oi.OrderId = @orderId",
            new SQLiteParameter("@orderId", orderId));
        
        foreach (DataRow row in dt.Rows)
        {
            items.Add(new OrderItem
            {
                Id = Convert.ToInt32(row["Id"]),
                OrderId = Convert.ToInt32(row["OrderId"]),
                ProductId = Convert.ToInt32(row["ProductId"]),
                ProductName = row["ProductName"].ToString() ?? "",
                Quantity = Convert.ToInt32(row["Quantity"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"])
            });
        }
        
        return items;
    }
    
    public void AddOrderItem(int orderId, int productId, decimal unitPrice, int quantity = 1)
    {
        // Check if item already exists
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT Id, Quantity FROM OrderItems WHERE OrderId = @orderId AND ProductId = @productId",
            new SQLiteParameter("@orderId", orderId),
            new SQLiteParameter("@productId", productId));
        
        if (dt.Rows.Count > 0)
        {
            // Update quantity
            var currentQty = Convert.ToInt32(dt.Rows[0]["Quantity"]);
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE OrderItems SET Quantity = @quantity WHERE OrderId = @orderId AND ProductId = @productId",
                new SQLiteParameter("@quantity", currentQty + quantity),
                new SQLiteParameter("@orderId", orderId),
                new SQLiteParameter("@productId", productId));
        }
        else
        {
            // Add new item
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@orderId, @productId, @quantity, @unitPrice)",
                new SQLiteParameter("@orderId", orderId),
                new SQLiteParameter("@productId", productId),
                new SQLiteParameter("@quantity", quantity),
                new SQLiteParameter("@unitPrice", unitPrice));
        }
        
        UpdateOrderTotal(orderId);
    }
    
    public void RemoveOrderItem(int orderItemId)
    {
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT OrderId FROM OrderItems WHERE Id = @id",
            new SQLiteParameter("@id", orderItemId));
        
        if (dt.Rows.Count == 0)
            return;
        
        var orderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
        
        DatabaseHelper.ExecuteNonQuery(
            "DELETE FROM OrderItems WHERE Id = @id",
            new SQLiteParameter("@id", orderItemId));
        
        UpdateOrderTotal(orderId);
    }
    
    public void UpdateOrderItemQuantity(int orderItemId, int quantity)
    {
        if (quantity <= 0)
        {
            RemoveOrderItem(orderItemId);
            return;
        }
        
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT OrderId FROM OrderItems WHERE Id = @id",
            new SQLiteParameter("@id", orderItemId));
        
        if (dt.Rows.Count == 0)
            return;
        
        var orderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
        
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE OrderItems SET Quantity = @quantity WHERE Id = @id",
            new SQLiteParameter("@quantity", quantity),
            new SQLiteParameter("@id", orderItemId));
        
        UpdateOrderTotal(orderId);
    }
    
    private void UpdateOrderTotal(int orderId)
    {
        var result = DatabaseHelper.ExecuteScalar(
            "SELECT SUM(Quantity * UnitPrice) FROM OrderItems WHERE OrderId = @orderId",
            new SQLiteParameter("@orderId", orderId));
        
        var total = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
        
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE Orders SET TotalAmount = @total WHERE Id = @id",
            new SQLiteParameter("@total", total),
            new SQLiteParameter("@id", orderId));
    }
    
    public void CloseOrder(int orderId, string paymentMethod = "Nakit")
    {
        DatabaseHelper.ExecuteNonQuery(
            "UPDATE Orders SET Status = 1, PaymentMethod = @paymentMethod WHERE Id = @id",
            new SQLiteParameter("@paymentMethod", paymentMethod),
            new SQLiteParameter("@id", orderId));
        
        // Update table status
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT TableId FROM Orders WHERE Id = @id",
            new SQLiteParameter("@id", orderId));
        
        if (dt.Rows.Count > 0)
        {
            var tableId = Convert.ToInt32(dt.Rows[0]["TableId"]);
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE Tables SET Status = 0 WHERE Id = @id",
                new SQLiteParameter("@id", tableId));
        }
    }
    
    public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
    {
        var orders = new List<Order>();
        var dt = DatabaseHelper.ExecuteQuery(
            @"SELECT Id, TableId, OrderDate, TotalAmount, Status, PaymentMethod 
              FROM Orders 
              WHERE date(OrderDate) BETWEEN date(@startDate) AND date(@endDate) 
              ORDER BY OrderDate DESC",
            new SQLiteParameter("@startDate", startDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@endDate", endDate.ToString("yyyy-MM-dd")));
        
        foreach (DataRow row in dt.Rows)
        {
            orders.Add(new Order
            {
                Id = Convert.ToInt32(row["Id"]),
                TableId = Convert.ToInt32(row["TableId"]),
                OrderDate = DateTime.Parse(row["OrderDate"].ToString()!),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                Status = (OrderStatus)Convert.ToInt32(row["Status"]),
                PaymentMethod = row["PaymentMethod"]?.ToString() ?? ""
            });
        }
        
        return orders;
    }
    
    public DataTable GetProductSalesReport(DateTime startDate, DateTime endDate)
    {
        return DatabaseHelper.ExecuteQuery(
            @"SELECT p.Name, SUM(oi.Quantity) as TotalQuantity, SUM(oi.Quantity * oi.UnitPrice) as TotalRevenue
              FROM OrderItems oi
              JOIN Orders o ON oi.OrderId = o.Id
              JOIN Products p ON oi.ProductId = p.Id
              WHERE date(o.OrderDate) BETWEEN date(@startDate) AND date(@endDate) AND o.Status = 1
              GROUP BY p.Name
              ORDER BY TotalQuantity DESC",
            new SQLiteParameter("@startDate", startDate.ToString("yyyy-MM-dd")),
            new SQLiteParameter("@endDate", endDate.ToString("yyyy-MM-dd")));
    }
}
