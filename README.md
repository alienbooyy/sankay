# Sankay POS - Restaurant Point of Sale System

A comprehensive Point of Sale (POS) system for restaurants built with C# and WinForms targeting .NET 8.0.

## Features

### 1. Main Page with Tables
- Touch-friendly table interface with visual status indicators
- Green buttons for available tables, red for occupied tables
- Click a table to open the order (adisyon) page
- Long-press functionality for table operations (move/merge)

### 2. Order Management (Adisyon)
- Add and remove products from orders
- Real-time order total calculation
- Four action buttons:
  - **Kapat (Close)**: Close the order form
  - **Alman Usulü (Split Payment)**: Split bill among multiple customers
  - **Ödeme Al (Payment)**: Process payment and close the order
  - **Yazdır (Print)**: Print order to kitchen/oven printers

### 3. Admin Panel
- Password-protected access (default password: 1234)
- End-of-day reporting with customizable date ranges
- View total revenue and individual product sales
- Export reports to Excel
- Print functionality for daily summaries

### 4. Table Management
- Add, remove, and rename tables
- Arrange table positions
- Manage table availability status

### 5. Product Management
- Add, edit, and remove products
- Set product prices and categories
- Touch-friendly product selection in orders

### 6. Raw Materials and Inventory
- Manage raw materials with units (grams, liters, pieces)
- Track current stock levels
- Set minimum stock thresholds
- Automatic low-stock warnings
- Cost tracking per unit

### 7. Recipe Management
- Link raw materials to products
- Define ingredient quantities for each product
- Automatic stock deduction when orders are completed
- View and update recipes

### 8. Inventory Tracking
- Real-time stock monitoring
- Visual warnings for low stock items
- Automatic inventory updates based on sales

### 9. Tablet Integration (Coming Soon)
- TCP/IP server for tablet connections on local network
- Tablets can access the POS at IP 192.168.1.35
- Order synchronization between devices

### 10. Printer Integration (Coming Soon)
- Support for SPENTA thermal printers
- Separate printing for kitchen and oven sections
- Properly formatted receipts

## Technology Stack

- **Framework**: .NET 8.0
- **UI**: Windows Forms
- **Database**: SQLite (local, offline-first)
- **Excel Export**: EPPlus
- **Language**: C# with nullable reference types

## Database Schema

The system uses SQLite with the following main tables:
- **Tables**: Restaurant table information and status
- **Products**: Menu items with pricing
- **RawMaterials**: Inventory items with stock tracking
- **Recipes**: Junction table linking products to raw materials
- **Orders**: Customer orders with status
- **OrderItems**: Individual items in each order
- **Settings**: Application configuration

## Getting Started

### Prerequisites
- Windows OS (Windows 10 or later recommended)
- .NET 8.0 SDK or Runtime

### Building from Source

```bash
# Clone the repository
git clone https://github.com/alienbooyy/sankay.git
cd sankay/SankayPOS

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### First Launch

On first launch, the application will:
1. Create the SQLite database (`sankay.db`) in the application directory
2. Initialize all necessary tables
3. Create 12 default tables (Masa 1 through Masa 12)
4. Set default admin password to "1234"

## Usage Guide

### Managing Tables
1. Click "Masa Yönetimi" on the main screen
2. Add new tables, rename existing ones, or remove tables
3. Changes are immediately reflected on the main screen

### Creating Products
1. Click "Ürün Yönetimi" on the main screen
2. Add products with names, prices, and categories
3. Products will appear in the order screen

### Setting Up Inventory
1. Click "Stok Yönetimi" to manage raw materials
2. Add items like "Kıyma" (minced meat), "Un" (flour), etc.
3. Set units (gr, lt, adet), current stock, and minimum thresholds

### Creating Recipes
1. Click "Reçete Yönetimi" to link raw materials to products
2. Select a product (e.g., Lahmacun)
3. Add ingredients with quantities (e.g., Kıyma 20g, Un 100g)
4. Stock will automatically deduct when orders are completed

### Taking Orders
1. Click on a table to open the order screen
2. Click products to add them to the order
3. Double-click items in the order to remove them
4. Use "Ödeme Al" to complete the order (deducts inventory)
5. Use "Alman Usulü" for split payments
6. Use "Yazdır" to send to kitchen printer

### Admin Reports
1. Click "Yönetici Paneli" and enter password (default: 1234)
2. Select date range for reports
3. View total revenue and product sales
4. Export to Excel for further analysis

## Configuration

### Changing Admin Password
The admin password is stored in the Settings table. To change it:
1. Open the `sankay.db` file with a SQLite browser
2. Update the value in Settings table where Key = 'AdminPassword'

### Tablet Server Settings
Settings for tablet integration are stored in the database:
- TabletServerEnabled: 'true' or 'false'
- TabletServerIP: '192.168.1.35'
- TabletServerPort: '8080'

## Project Structure

```
SankayPOS/
├── Database/
│   ├── DatabaseHelper.cs      # Database initialization and helpers
│   └── Repositories.cs        # Data access layer
├── Forms/
│   ├── MainForm.cs            # Main table selection screen
│   ├── OrderForm.cs           # Order management (adisyon)
│   ├── AdminForm.cs           # Admin panel with reports
│   ├── TableManagementForm.cs # Table management
│   ├── ProductManagementForm.cs # Product management
│   ├── InventoryManagementForm.cs # Inventory/raw materials
│   ├── RecipeManagementForm.cs # Recipe management
│   └── SplitPaymentForm.cs    # Split payment dialog
├── Models/
│   └── Models.cs              # Data models
└── Program.cs                 # Application entry point
```

## Future Enhancements

- [ ] QR code menu functionality
- [ ] Customer loyalty program
- [ ] Online ordering integration
- [ ] Multi-language support
- [ ] Cloud synchronization
- [ ] Advanced analytics and charts
- [ ] Employee management and permissions
- [ ] Table reservation system

## License

This project is open source and available for modification and distribution.

## Support

For issues, questions, or contributions, please open an issue on GitHub.
