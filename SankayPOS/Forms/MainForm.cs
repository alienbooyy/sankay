using SankayPOS.Database;
using SankayPOS.Models;

namespace SankayPOS.Forms;

public partial class MainForm : Form
{
    private readonly TableRepository _tableRepo = new();
    private readonly OrderRepository _orderRepo = new();
    private FlowLayoutPanel? _tablePanel;
    private System.Windows.Forms.Timer? _longPressTimer;
    private Button? _longPressButton;
    
    public MainForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        LoadTables();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        // MainForm
        this.ClientSize = new Size(1024, 768);
        this.Name = "MainForm";
        this.Text = "Sankay POS - Ana Sayfa";
        this.WindowState = FormWindowState.Maximized;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        // Top menu panel
        var menuPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(41, 128, 185)
        };
        
        var btnAdmin = new Button
        {
            Text = "Yönetici Paneli",
            Location = new Point(10, 10),
            Size = new Size(150, 40),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAdmin.Click += BtnAdmin_Click;
        
        var btnTableManagement = new Button
        {
            Text = "Masa Yönetimi",
            Location = new Point(170, 10),
            Size = new Size(150, 40),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnTableManagement.Click += BtnTableManagement_Click;
        
        var btnProductManagement = new Button
        {
            Text = "Ürün Yönetimi",
            Location = new Point(330, 10),
            Size = new Size(150, 40),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnProductManagement.Click += BtnProductManagement_Click;
        
        var btnInventory = new Button
        {
            Text = "Stok Yönetimi",
            Location = new Point(490, 10),
            Size = new Size(150, 40),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnInventory.Click += BtnInventory_Click;
        
        var btnRecipes = new Button
        {
            Text = "Reçete Yönetimi",
            Location = new Point(650, 10),
            Size = new Size(150, 40),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnRecipes.Click += BtnRecipes_Click;
        
        menuPanel.Controls.Add(btnAdmin);
        menuPanel.Controls.Add(btnTableManagement);
        menuPanel.Controls.Add(btnProductManagement);
        menuPanel.Controls.Add(btnInventory);
        menuPanel.Controls.Add(btnRecipes);
        
        // Table panel
        _tablePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(20),
            BackColor = Color.White
        };
        
        this.Controls.Add(_tablePanel);
        this.Controls.Add(menuPanel);
        
        // Initialize long press timer
        _longPressTimer = new System.Windows.Forms.Timer
        {
            Interval = 1000 // 1 second for long press
        };
        _longPressTimer.Tick += LongPressTimer_Tick;
    }
    
    private void LoadTables()
    {
        _tablePanel?.Controls.Clear();
        var tables = _tableRepo.GetAllTables();
        
        // If no tables exist, create some default ones
        if (tables.Count == 0)
        {
            for (int i = 1; i <= 12; i++)
            {
                _tableRepo.AddTable($"Masa {i}", i);
            }
            tables = _tableRepo.GetAllTables();
        }
        
        foreach (var table in tables)
        {
            var btnTable = new Button
            {
                Text = table.Name,
                Size = new Size(150, 120),
                Margin = new Padding(10),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = table.Status == TableStatus.Available ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = table
            };
            
            btnTable.Click += BtnTable_Click;
            btnTable.MouseDown += BtnTable_MouseDown;
            btnTable.MouseUp += BtnTable_MouseUp;
            
            _tablePanel?.Controls.Add(btnTable);
        }
    }
    
    private void BtnTable_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not Table table)
            return;
        
        var orderForm = new OrderForm(table);
        orderForm.FormClosed += (s, args) => LoadTables();
        orderForm.ShowDialog();
    }
    
    private void BtnTable_MouseDown(object? sender, MouseEventArgs e)
    {
        if (sender is Button btn)
        {
            _longPressButton = btn;
            _longPressTimer?.Start();
        }
    }
    
    private void BtnTable_MouseUp(object? sender, MouseEventArgs e)
    {
        _longPressTimer?.Stop();
        _longPressButton = null;
    }
    
    private void LongPressTimer_Tick(object? sender, EventArgs e)
    {
        _longPressTimer?.Stop();
        
        if (_longPressButton?.Tag is Table table)
        {
            ShowTableOptions(table);
        }
        
        _longPressButton = null;
    }
    
    private void ShowTableOptions(Table table)
    {
        var menu = new ContextMenuStrip();
        
        var moveItem = new ToolStripMenuItem("Masa Taşı");
        moveItem.Click += (s, e) =>
        {
            MessageBox.Show("Masa taşıma özelliği geliştirilme aşamasında.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        
        var mergeItem = new ToolStripMenuItem("Masa Birleştir");
        mergeItem.Click += (s, e) =>
        {
            MessageBox.Show("Masa birleştirme özelliği geliştirilme aşamasında.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        
        menu.Items.Add(moveItem);
        menu.Items.Add(mergeItem);
        menu.Show(Cursor.Position);
    }
    
    private void BtnAdmin_Click(object? sender, EventArgs e)
    {
        var adminForm = new AdminForm();
        adminForm.ShowDialog();
    }
    
    private void BtnTableManagement_Click(object? sender, EventArgs e)
    {
        var tableManagementForm = new TableManagementForm();
        tableManagementForm.FormClosed += (s, args) => LoadTables();
        tableManagementForm.ShowDialog();
    }
    
    private void BtnProductManagement_Click(object? sender, EventArgs e)
    {
        var productManagementForm = new ProductManagementForm();
        productManagementForm.ShowDialog();
    }
    
    private void BtnInventory_Click(object? sender, EventArgs e)
    {
        var inventoryForm = new InventoryManagementForm();
        inventoryForm.ShowDialog();
    }
    
    private void BtnRecipes_Click(object? sender, EventArgs e)
    {
        var recipeForm = new RecipeManagementForm();
        recipeForm.ShowDialog();
    }
}
