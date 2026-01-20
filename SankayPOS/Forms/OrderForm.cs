using SankayPOS.Database;
using SankayPOS.Models;
using System.Data;

namespace SankayPOS.Forms;

public partial class OrderForm : Form
{
    private readonly Table _table;
    private readonly OrderRepository _orderRepo = new();
    private readonly ProductRepository _productRepo = new();
    private readonly RecipeRepository _recipeRepo = new();
    private Order? _currentOrder;
    private ListBox? _orderItemsList;
    private FlowLayoutPanel? _productPanel;
    private Label? _totalLabel;
    
    public OrderForm(Table table)
    {
        _table = table;
        InitializeComponent();
        InitializeCustomComponents();
        LoadOrder();
        LoadProducts();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        // OrderForm
        this.ClientSize = new Size(1200, 800);
        this.Name = "OrderForm";
        this.Text = $"Adisyon - {_table.Name}";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        // Left panel - Products
        var leftPanel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 700,
            BackColor = Color.White
        };
        
        var lblProducts = new Label
        {
            Text = "Ürünler",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        _productPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(10),
            BackColor = Color.White
        };
        
        leftPanel.Controls.Add(_productPanel);
        leftPanel.Controls.Add(lblProducts);
        
        // Right panel - Order items
        var rightPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(236, 240, 241)
        };
        
        var lblOrder = new Label
        {
            Text = "Sipariş",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        _orderItemsList = new ListBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 12),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None
        };
        _orderItemsList.DoubleClick += OrderItemsList_DoubleClick;
        
        var totalPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(52, 73, 94)
        };
        
        _totalLabel = new Label
        {
            Text = "Toplam: 0.00 TL",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.White
        };
        
        totalPanel.Controls.Add(_totalLabel);
        
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = Color.FromArgb(236, 240, 241)
        };
        
        var btnClose = new Button
        {
            Text = "Kapat",
            Location = new Point(10, 15),
            Size = new Size(110, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnClose.Click += BtnClose_Click;
        
        var btnSplit = new Button
        {
            Text = "Alman Usulü",
            Location = new Point(130, 15),
            Size = new Size(110, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(243, 156, 18),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnSplit.Click += BtnSplit_Click;
        
        var btnPayment = new Button
        {
            Text = "Ödeme Al",
            Location = new Point(250, 15),
            Size = new Size(110, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnPayment.Click += BtnPayment_Click;
        
        var btnPrint = new Button
        {
            Text = "Yazdır",
            Location = new Point(370, 15),
            Size = new Size(110, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnPrint.Click += BtnPrint_Click;
        
        buttonPanel.Controls.Add(btnClose);
        buttonPanel.Controls.Add(btnSplit);
        buttonPanel.Controls.Add(btnPayment);
        buttonPanel.Controls.Add(btnPrint);
        
        rightPanel.Controls.Add(_orderItemsList);
        rightPanel.Controls.Add(totalPanel);
        rightPanel.Controls.Add(buttonPanel);
        rightPanel.Controls.Add(lblOrder);
        
        this.Controls.Add(rightPanel);
        this.Controls.Add(leftPanel);
    }
    
    private void LoadOrder()
    {
        _currentOrder = _orderRepo.GetOpenOrderByTable(_table.Id);
        
        if (_currentOrder == null)
        {
            var orderId = _orderRepo.CreateOrder(_table.Id);
            _currentOrder = _orderRepo.GetOpenOrderByTable(_table.Id);
            
            // Update table status
            var tableRepo = new TableRepository();
            tableRepo.UpdateTableStatus(_table.Id, TableStatus.Occupied);
        }
        
        UpdateOrderDisplay();
    }
    
    private void LoadProducts()
    {
        _productPanel?.Controls.Clear();
        var products = _productRepo.GetAllProducts();
        
        foreach (var product in products)
        {
            var btnProduct = new Button
            {
                Text = $"{product.Name}\n{product.Price:C}",
                Size = new Size(150, 100),
                Margin = new Padding(5),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = product
            };
            
            btnProduct.Click += BtnProduct_Click;
            _productPanel?.Controls.Add(btnProduct);
        }
    }
    
    private void BtnProduct_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not Product product || _currentOrder == null)
            return;
        
        _orderRepo.AddOrderItem(_currentOrder.Id, product.Id, product.Price);
        
        // Reload order
        _currentOrder = _orderRepo.GetOpenOrderByTable(_table.Id);
        UpdateOrderDisplay();
    }
    
    private void UpdateOrderDisplay()
    {
        _orderItemsList?.Items.Clear();
        
        if (_currentOrder == null)
            return;
        
        foreach (var item in _currentOrder.Items)
        {
            var displayText = $"{item.Quantity}x {item.ProductName} - {item.TotalPrice:C}";
            _orderItemsList?.Items.Add(new OrderItemDisplay { Item = item, DisplayText = displayText });
        }
        
        if (_totalLabel != null)
            _totalLabel.Text = $"Toplam: {_currentOrder.TotalAmount:C}";
    }
    
    private void OrderItemsList_DoubleClick(object? sender, EventArgs e)
    {
        if (_orderItemsList?.SelectedItem is not OrderItemDisplay display)
            return;
        
        var result = MessageBox.Show(
            $"{display.Item.ProductName} ürününü kaldırmak istiyor musunuz?",
            "Ürün Kaldır",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _orderRepo.RemoveOrderItem(display.Item.Id);
            _currentOrder = _orderRepo.GetOpenOrderByTable(_table.Id);
            UpdateOrderDisplay();
        }
    }
    
    private void BtnClose_Click(object? sender, EventArgs e)
    {
        this.Close();
    }
    
    private void BtnSplit_Click(object? sender, EventArgs e)
    {
        if (_currentOrder == null || _currentOrder.Items.Count == 0)
        {
            MessageBox.Show("Sipariş boş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var splitForm = new SplitPaymentForm(_currentOrder);
        splitForm.ShowDialog();
        
        _currentOrder = _orderRepo.GetOpenOrderByTable(_table.Id);
        UpdateOrderDisplay();
    }
    
    private void BtnPayment_Click(object? sender, EventArgs e)
    {
        if (_currentOrder == null || _currentOrder.Items.Count == 0)
        {
            MessageBox.Show("Sipariş boş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var result = MessageBox.Show(
            $"Toplam tutar: {_currentOrder.TotalAmount:C}\n\nÖdemeyi almak istiyor musunuz?",
            "Ödeme Al",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            // Deduct stock for products
            foreach (var item in _currentOrder.Items)
            {
                _recipeRepo.DeductStockForProduct(item.ProductId, item.Quantity);
            }
            
            _orderRepo.CloseOrder(_currentOrder.Id);
            MessageBox.Show("Ödeme alındı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
    
    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        if (_currentOrder == null || _currentOrder.Items.Count == 0)
        {
            MessageBox.Show("Sipariş boş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        // Print order to kitchen/oven printers
        PrintOrder(_currentOrder);
        MessageBox.Show("Sipariş yazıcıya gönderildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void PrintOrder(Order order)
    {
        // This would integrate with SPENTA thermal printers
        // For now, we'll just create a simple print preview
        var printText = $"MASA: {_table.Name}\n";
        printText += $"TARİH: {order.OrderDate:dd.MM.yyyy HH:mm}\n";
        printText += new string('-', 30) + "\n";
        
        foreach (var item in order.Items)
        {
            printText += $"{item.Quantity}x {item.ProductName}\n";
        }
        
        printText += new string('-', 30) + "\n";
        printText += $"TOPLAM: {order.TotalAmount:C}\n";
        
        // In production, this would send to actual thermal printer
        System.Diagnostics.Debug.WriteLine(printText);
    }
    
    private class OrderItemDisplay
    {
        public OrderItem Item { get; set; } = null!;
        public string DisplayText { get; set; } = string.Empty;
        
        public override string ToString() => DisplayText;
    }
}
