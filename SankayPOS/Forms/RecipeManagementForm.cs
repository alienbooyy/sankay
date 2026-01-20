using SankayPOS.Database;
using SankayPOS.Models;

namespace SankayPOS.Forms;

public partial class RecipeManagementForm : Form
{
    private readonly ProductRepository _productRepo = new();
    private readonly RawMaterialRepository _rawMaterialRepo = new();
    private readonly RecipeRepository _recipeRepo = new();
    private ComboBox? _cmbProducts;
    private ComboBox? _cmbRawMaterials;
    private TextBox? _txtQuantity;
    private ListBox? _lstRecipes;
    
    public RecipeManagementForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        LoadProducts();
        LoadRawMaterials();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(800, 600);
        this.Name = "RecipeManagementForm";
        this.Text = "Reçete Yönetimi";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        var lblTitle = new Label
        {
            Text = "Reçete Yönetimi",
            Location = new Point(20, 20),
            Size = new Size(760, 30),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        var lblProduct = new Label
        {
            Text = "Ürün Seç:",
            Location = new Point(20, 70),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _cmbProducts = new ComboBox
        {
            Location = new Point(130, 70),
            Size = new Size(300, 25),
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbProducts.SelectedIndexChanged += CmbProducts_SelectedIndexChanged;
        
        var lblRecipes = new Label
        {
            Text = "Reçete İçeriği:",
            Location = new Point(20, 110),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
        
        _lstRecipes = new ListBox
        {
            Location = new Point(20, 140),
            Size = new Size(410, 300),
            Font = new Font("Segoe UI", 10)
        };
        _lstRecipes.DoubleClick += LstRecipes_DoubleClick;
        
        var lblAddRecipe = new Label
        {
            Text = "Hammadde Ekle:",
            Location = new Point(450, 110),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
        
        var lblRawMaterial = new Label
        {
            Text = "Hammadde:",
            Location = new Point(450, 150),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _cmbRawMaterials = new ComboBox
        {
            Location = new Point(450, 180),
            Size = new Size(330, 25),
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        
        var lblQuantity = new Label
        {
            Text = "Miktar:",
            Location = new Point(450, 220),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtQuantity = new TextBox
        {
            Location = new Point(450, 250),
            Size = new Size(330, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var btnAddIngredient = new Button
        {
            Text = "Hammaddeyi Ekle",
            Location = new Point(450, 290),
            Size = new Size(330, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAddIngredient.Click += BtnAddIngredient_Click;
        
        var lblInfo = new Label
        {
            Text = "Not: Reçetedeki bir hammaddeyi silmek için\nlistede çift tıklayın.",
            Location = new Point(450, 350),
            Size = new Size(330, 50),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Gray
        };
        
        this.Controls.Add(lblTitle);
        this.Controls.Add(lblProduct);
        this.Controls.Add(_cmbProducts);
        this.Controls.Add(lblRecipes);
        this.Controls.Add(_lstRecipes);
        this.Controls.Add(lblAddRecipe);
        this.Controls.Add(lblRawMaterial);
        this.Controls.Add(_cmbRawMaterials);
        this.Controls.Add(lblQuantity);
        this.Controls.Add(_txtQuantity);
        this.Controls.Add(btnAddIngredient);
        this.Controls.Add(lblInfo);
    }
    
    private void LoadProducts()
    {
        _cmbProducts?.Items.Clear();
        var products = _productRepo.GetAllProducts();
        
        foreach (var product in products)
        {
            _cmbProducts?.Items.Add(new ProductDisplay { Product = product });
        }
        
        if (_cmbProducts != null && _cmbProducts.Items.Count > 0)
            _cmbProducts.SelectedIndex = 0;
    }
    
    private void LoadRawMaterials()
    {
        _cmbRawMaterials?.Items.Clear();
        var materials = _rawMaterialRepo.GetAllRawMaterials();
        
        foreach (var material in materials)
        {
            _cmbRawMaterials?.Items.Add(new MaterialDisplay { Material = material });
        }
        
        if (_cmbRawMaterials != null && _cmbRawMaterials.Items.Count > 0)
            _cmbRawMaterials.SelectedIndex = 0;
    }
    
    private void CmbProducts_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadRecipes();
    }
    
    private void LoadRecipes()
    {
        _lstRecipes?.Items.Clear();
        
        if (_cmbProducts?.SelectedItem is not ProductDisplay display)
            return;
        
        var recipes = _recipeRepo.GetRecipesByProduct(display.Product.Id);
        
        foreach (var recipe in recipes)
        {
            _lstRecipes?.Items.Add(new RecipeDisplay { Recipe = recipe });
        }
    }
    
    private void BtnAddIngredient_Click(object? sender, EventArgs e)
    {
        if (_cmbProducts?.SelectedItem is not ProductDisplay productDisplay)
        {
            MessageBox.Show("Lütfen bir ürün seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (_cmbRawMaterials?.SelectedItem is not MaterialDisplay materialDisplay)
        {
            MessageBox.Show("Lütfen bir hammadde seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (!decimal.TryParse(_txtQuantity?.Text, out decimal quantity) || quantity <= 0)
        {
            MessageBox.Show("Geçerli bir miktar girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _recipeRepo.AddRecipe(productDisplay.Product.Id, materialDisplay.Material.Id, quantity);
        LoadRecipes();
        
        if (_txtQuantity != null)
            _txtQuantity.Text = "";
        
        MessageBox.Show("Hammadde reçeteye eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void LstRecipes_DoubleClick(object? sender, EventArgs e)
    {
        if (_lstRecipes?.SelectedItem is not RecipeDisplay display)
            return;
        
        var result = MessageBox.Show(
            $"{display.Recipe.RawMaterialName} hammaddesini reçeteden çıkarmak istiyor musunuz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _recipeRepo.DeleteRecipe(display.Recipe.Id);
            LoadRecipes();
            MessageBox.Show("Hammadde reçeteden çıkarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
    private class ProductDisplay
    {
        public Product Product { get; set; } = null!;
        public override string ToString() => Product.Name;
    }
    
    private class MaterialDisplay
    {
        public RawMaterial Material { get; set; } = null!;
        public override string ToString() => $"{Material.Name} ({Material.Unit})";
    }
    
    private class RecipeDisplay
    {
        public Recipe Recipe { get; set; } = null!;
        public override string ToString() => $"{Recipe.RawMaterialName} - {Recipe.Quantity} {Recipe.Unit}";
    }
}
