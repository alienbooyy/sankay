using SankayPOS.Database;
using SankayPOS.Models;

namespace SankayPOS.Forms;

public partial class ProductManagementForm : Form
{
    private readonly ProductRepository _productRepo = new();
    private ListBox? _lstProducts;
    private TextBox? _txtName;
    private TextBox? _txtPrice;
    private TextBox? _txtCategory;
    
    public ProductManagementForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        LoadProducts();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(700, 500);
        this.Name = "ProductManagementForm";
        this.Text = "Ürün Yönetimi";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        var lblTitle = new Label
        {
            Text = "Ürün Yönetimi",
            Location = new Point(20, 20),
            Size = new Size(660, 30),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        _lstProducts = new ListBox
        {
            Location = new Point(20, 70),
            Size = new Size(350, 350),
            Font = new Font("Segoe UI", 10)
        };
        _lstProducts.SelectedIndexChanged += LstProducts_SelectedIndexChanged;
        
        var lblName = new Label
        {
            Text = "Ürün Adı:",
            Location = new Point(390, 70),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtName = new TextBox
        {
            Location = new Point(390, 100),
            Size = new Size(290, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblPrice = new Label
        {
            Text = "Fiyat (TL):",
            Location = new Point(390, 135),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtPrice = new TextBox
        {
            Location = new Point(390, 165),
            Size = new Size(290, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var lblCategory = new Label
        {
            Text = "Kategori:",
            Location = new Point(390, 200),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtCategory = new TextBox
        {
            Location = new Point(390, 230),
            Size = new Size(290, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var btnAdd = new Button
        {
            Text = "Yeni Ürün Ekle",
            Location = new Point(390, 270),
            Size = new Size(290, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAdd.Click += BtnAdd_Click;
        
        var btnUpdate = new Button
        {
            Text = "Ürünü Güncelle",
            Location = new Point(390, 315),
            Size = new Size(290, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnUpdate.Click += BtnUpdate_Click;
        
        var btnDelete = new Button
        {
            Text = "Ürünü Sil",
            Location = new Point(390, 360),
            Size = new Size(290, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnDelete.Click += BtnDelete_Click;
        
        this.Controls.Add(lblTitle);
        this.Controls.Add(_lstProducts);
        this.Controls.Add(lblName);
        this.Controls.Add(_txtName);
        this.Controls.Add(lblPrice);
        this.Controls.Add(_txtPrice);
        this.Controls.Add(lblCategory);
        this.Controls.Add(_txtCategory);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnUpdate);
        this.Controls.Add(btnDelete);
    }
    
    private void LoadProducts()
    {
        _lstProducts?.Items.Clear();
        var products = _productRepo.GetAllProducts();
        
        foreach (var product in products)
        {
            _lstProducts?.Items.Add(new ProductDisplay { Product = product });
        }
    }
    
    private void LstProducts_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_lstProducts?.SelectedItem is ProductDisplay display)
        {
            if (_txtName != null) _txtName.Text = display.Product.Name;
            if (_txtPrice != null) _txtPrice.Text = display.Product.Price.ToString();
            if (_txtCategory != null) _txtCategory.Text = display.Product.Category;
        }
    }
    
    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtName?.Text))
        {
            MessageBox.Show("Ürün adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (!decimal.TryParse(_txtPrice?.Text, out decimal price) || price < 0)
        {
            MessageBox.Show("Geçerli bir fiyat girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _productRepo.AddProduct(_txtName.Text, price, _txtCategory?.Text ?? "");
        LoadProducts();
        ClearFields();
        MessageBox.Show("Ürün eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (_lstProducts?.SelectedItem is not ProductDisplay display)
        {
            MessageBox.Show("Lütfen bir ürün seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(_txtName?.Text))
        {
            MessageBox.Show("Ürün adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (!decimal.TryParse(_txtPrice?.Text, out decimal price) || price < 0)
        {
            MessageBox.Show("Geçerli bir fiyat girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _productRepo.UpdateProduct(display.Product.Id, _txtName.Text, price, _txtCategory?.Text ?? "");
        LoadProducts();
        MessageBox.Show("Ürün güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_lstProducts?.SelectedItem is not ProductDisplay display)
        {
            MessageBox.Show("Lütfen bir ürün seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var result = MessageBox.Show(
            $"{display.Product.Name} ürününü silmek istediğinizden emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _productRepo.DeleteProduct(display.Product.Id);
            LoadProducts();
            ClearFields();
            MessageBox.Show("Ürün silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
    private void ClearFields()
    {
        if (_txtName != null) _txtName.Text = "";
        if (_txtPrice != null) _txtPrice.Text = "";
        if (_txtCategory != null) _txtCategory.Text = "";
    }
    
    private class ProductDisplay
    {
        public Product Product { get; set; } = null!;
        public override string ToString() => $"{Product.Name} - {Product.Price:C}";
    }
}
