using System.Data;
using System.Data.SQLite;

namespace SankayPOS.Database;

public class DatabaseHelper
{
    // Configuration - can be changed to point to different database locations
    public static string DatabaseFileName { get; set; } = "sankay.db";
    private static string ConnectionString => $"Data Source={DatabaseFileName};Version=3;";
    
    public static void InitializeDatabase()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        
        // Tables table
        string createTablesTable = @"
            CREATE TABLE IF NOT EXISTS Tables (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Status INTEGER DEFAULT 0,
                Position INTEGER DEFAULT 0
            )";
        
        // Products table
        string createProductsTable = @"
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price REAL NOT NULL,
                Category TEXT
            )";
        
        // RawMaterials table
        string createRawMaterialsTable = @"
            CREATE TABLE IF NOT EXISTS RawMaterials (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Unit TEXT NOT NULL,
                CurrentStock REAL DEFAULT 0,
                MinimumStock REAL DEFAULT 0,
                CostPerUnit REAL DEFAULT 0
            )";
        
        // Recipes table (junction table)
        string createRecipesTable = @"
            CREATE TABLE IF NOT EXISTS Recipes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProductId INTEGER NOT NULL,
                RawMaterialId INTEGER NOT NULL,
                Quantity REAL NOT NULL,
                FOREIGN KEY(ProductId) REFERENCES Products(Id),
                FOREIGN KEY(RawMaterialId) REFERENCES RawMaterials(Id)
            )";
        
        // Orders table
        string createOrdersTable = @"
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TableId INTEGER NOT NULL,
                OrderDate TEXT NOT NULL,
                TotalAmount REAL DEFAULT 0,
                Status INTEGER DEFAULT 0,
                PaymentMethod TEXT,
                FOREIGN KEY(TableId) REFERENCES Tables(Id)
            )";
        
        // OrderItems table
        string createOrderItemsTable = @"
            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                UnitPrice REAL NOT NULL,
                FOREIGN KEY(OrderId) REFERENCES Orders(Id),
                FOREIGN KEY(ProductId) REFERENCES Products(Id)
            )";
        
        // Settings table
        string createSettingsTable = @"
            CREATE TABLE IF NOT EXISTS Settings (
                Key TEXT PRIMARY KEY,
                Value TEXT
            )";
        
        using var cmd = connection.CreateCommand();
        cmd.CommandText = createTablesTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createProductsTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createRawMaterialsTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createRecipesTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createOrdersTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createOrderItemsTable;
        cmd.ExecuteNonQuery();
        
        cmd.CommandText = createSettingsTable;
        cmd.ExecuteNonQuery();
        
        // Insert default admin password if not exists
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('AdminPassword', '1234')";
        cmd.ExecuteNonQuery();
        
        // Insert default tablet server settings
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('TabletServerEnabled', 'false')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('TabletServerIP', '192.168.1.35')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('TabletServerPort', '8080')";
        cmd.ExecuteNonQuery();
        
        // Insert default printer settings
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('KitchenPrinterIP', '192.168.1.100')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('OvenPrinterIP', '192.168.1.101')";
        cmd.ExecuteNonQuery();
    }
    
    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(ConnectionString);
    }
    
    public static void ExecuteNonQuery(string query, params SQLiteParameter[] parameters)
    {
        using var connection = GetConnection();
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        cmd.ExecuteNonQuery();
    }
    
    public static DataTable ExecuteQuery(string query, params SQLiteParameter[] parameters)
    {
        using var connection = GetConnection();
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        
        using var adapter = new SQLiteDataAdapter(cmd);
        var dataTable = new DataTable();
        adapter.Fill(dataTable);
        return dataTable;
    }
    
    public static object? ExecuteScalar(string query, params SQLiteParameter[] parameters)
    {
        using var connection = GetConnection();
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }
}
