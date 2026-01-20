using SankayPOS.Database;
using SankayPOS.Models;

namespace SankayPOS.Forms;

public partial class InventoryManagementForm : Form
{
    private readonly RawMaterialRepository _rawMaterialRepo = new();
    private ListBox? _lstMaterials;
    private TextBox? _txtName;
    private TextBox? _txtUnit;
    private TextBox? _txtCurrentStock;
    private TextBox? _txtMinimumStock;
    private TextBox? _txtCostPerUnit;
    private Label? _lblWarnings;
    
    public InventoryManagementForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        LoadMaterials();
        CheckLowStock();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(800, 600);
        this.Name = "InventoryManagementForm";
        this.Text = "Stok Yönetimi";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        var lblTitle = new Label
        {
            Text = "Hammadde ve Stok Yönetimi",
            Location = new Point(20, 20),
            Size = new Size(760, 30),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        _lblWarnings = new Label
        {
            Location = new Point(20, 60),
            Size = new Size(760, 40),
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.Red,
            TextAlign = ContentAlignment.MiddleLeft
        };
        
        _lstMaterials = new ListBox
        {
            Location = new Point(20, 110),
            Size = new Size(400, 400),
            Font = new Font("Segoe UI", 10)
        };
        _lstMaterials.SelectedIndexChanged += LstMaterials_SelectedIndexChanged;
        
        var lblName = new Label
        {
            Text = "Hammadde Adı:",
            Location = new Point(440, 110),
            Size = new Size(120, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtName = new TextBox
        {
            Location = new Point(440, 140),
            Size = new Size(340, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblUnit = new Label
        {
            Text = "Birim (gr, lt, adet):",
            Location = new Point(440, 175),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtUnit = new TextBox
        {
            Location = new Point(440, 205),
            Size = new Size(340, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblCurrentStock = new Label
        {
            Text = "Mevcut Stok:",
            Location = new Point(440, 240),
            Size = new Size(120, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtCurrentStock = new TextBox
        {
            Location = new Point(440, 270),
            Size = new Size(340, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblMinimumStock = new Label
        {
            Text = "Minimum Stok:",
            Location = new Point(440, 305),
            Size = new Size(120, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtMinimumStock = new TextBox
        {
            Location = new Point(440, 335),
            Size = new Size(340, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblCostPerUnit = new Label
        {
            Text = "Birim Maliyet (TL):",
            Location = new Point(440, 370),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtCostPerUnit = new TextBox
        {
            Location = new Point(440, 400),
            Size = new Size(340, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var btnAdd = new Button
        {
            Text = "Ekle",
            Location = new Point(440, 440),
            Size = new Size(105, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAdd.Click += BtnAdd_Click;
        
        var btnUpdate = new Button
        {
            Text = "Güncelle",
            Location = new Point(555, 440),
            Size = new Size(105, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnUpdate.Click += BtnUpdate_Click;
        
        var btnDelete = new Button
        {
            Text = "Sil",
            Location = new Point(670, 440),
            Size = new Size(110, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnDelete.Click += BtnDelete_Click;
        
        this.Controls.Add(lblTitle);
        this.Controls.Add(_lblWarnings);
        this.Controls.Add(_lstMaterials);
        this.Controls.Add(lblName);
        this.Controls.Add(_txtName);
        this.Controls.Add(lblUnit);
        this.Controls.Add(_txtUnit);
        this.Controls.Add(lblCurrentStock);
        this.Controls.Add(_txtCurrentStock);
        this.Controls.Add(lblMinimumStock);
        this.Controls.Add(_txtMinimumStock);
        this.Controls.Add(lblCostPerUnit);
        this.Controls.Add(_txtCostPerUnit);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnUpdate);
        this.Controls.Add(btnDelete);
    }
    
    private void LoadMaterials()
    {
        _lstMaterials?.Items.Clear();
        var materials = _rawMaterialRepo.GetAllRawMaterials();
        
        foreach (var material in materials)
        {
            _lstMaterials?.Items.Add(new MaterialDisplay { Material = material });
        }
    }
    
    private void CheckLowStock()
    {
        var materials = _rawMaterialRepo.GetAllRawMaterials();
        var lowStockItems = materials.Where(m => m.CurrentStock < m.MinimumStock).ToList();
        
        if (lowStockItems.Count > 0)
        {
            var warningText = "⚠️ DÜŞÜK STOK UYARISI: " + string.Join(", ", lowStockItems.Select(m => m.Name));
            if (_lblWarnings != null)
                _lblWarnings.Text = warningText;
        }
        else
        {
            if (_lblWarnings != null)
            {
                _lblWarnings.Text = "✓ Tüm stoklar yeterli seviyede.";
                _lblWarnings.ForeColor = Color.Green;
            }
        }
    }
    
    private void LstMaterials_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_lstMaterials?.SelectedItem is MaterialDisplay display)
        {
            if (_txtName != null) _txtName.Text = display.Material.Name;
            if (_txtUnit != null) _txtUnit.Text = display.Material.Unit;
            if (_txtCurrentStock != null) _txtCurrentStock.Text = display.Material.CurrentStock.ToString();
            if (_txtMinimumStock != null) _txtMinimumStock.Text = display.Material.MinimumStock.ToString();
            if (_txtCostPerUnit != null) _txtCostPerUnit.Text = display.Material.CostPerUnit.ToString();
        }
    }
    
    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtName?.Text) || string.IsNullOrWhiteSpace(_txtUnit?.Text))
        {
            MessageBox.Show("Hammadde adı ve birim boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (!decimal.TryParse(_txtCurrentStock?.Text, out decimal currentStock) || currentStock < 0 ||
            !decimal.TryParse(_txtMinimumStock?.Text, out decimal minimumStock) || minimumStock < 0 ||
            !decimal.TryParse(_txtCostPerUnit?.Text, out decimal costPerUnit) || costPerUnit < 0)
        {
            MessageBox.Show("Geçerli değerler girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _rawMaterialRepo.AddRawMaterial(_txtName.Text, _txtUnit.Text, currentStock, minimumStock, costPerUnit);
        LoadMaterials();
        CheckLowStock();
        ClearFields();
        MessageBox.Show("Hammadde eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (_lstMaterials?.SelectedItem is not MaterialDisplay display)
        {
            MessageBox.Show("Lütfen bir hammadde seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(_txtName?.Text) || string.IsNullOrWhiteSpace(_txtUnit?.Text))
        {
            MessageBox.Show("Hammadde adı ve birim boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (!decimal.TryParse(_txtCurrentStock?.Text, out decimal currentStock) || currentStock < 0 ||
            !decimal.TryParse(_txtMinimumStock?.Text, out decimal minimumStock) || minimumStock < 0 ||
            !decimal.TryParse(_txtCostPerUnit?.Text, out decimal costPerUnit) || costPerUnit < 0)
        {
            MessageBox.Show("Geçerli değerler girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _rawMaterialRepo.UpdateRawMaterial(display.Material.Id, _txtName.Text, _txtUnit.Text, currentStock, minimumStock, costPerUnit);
        LoadMaterials();
        CheckLowStock();
        MessageBox.Show("Hammadde güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_lstMaterials?.SelectedItem is not MaterialDisplay display)
        {
            MessageBox.Show("Lütfen bir hammadde seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var result = MessageBox.Show(
            $"{display.Material.Name} hammaddesini silmek istediğinizden emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _rawMaterialRepo.DeleteRawMaterial(display.Material.Id);
            LoadMaterials();
            CheckLowStock();
            ClearFields();
            MessageBox.Show("Hammadde silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
    private void ClearFields()
    {
        if (_txtName != null) _txtName.Text = "";
        if (_txtUnit != null) _txtUnit.Text = "";
        if (_txtCurrentStock != null) _txtCurrentStock.Text = "";
        if (_txtMinimumStock != null) _txtMinimumStock.Text = "";
        if (_txtCostPerUnit != null) _txtCostPerUnit.Text = "";
    }
    
    private class MaterialDisplay
    {
        public RawMaterial Material { get; set; } = null!;
        public override string ToString() => $"{Material.Name} - {Material.CurrentStock} {Material.Unit} (Min: {Material.MinimumStock})";
    }
}
