using SankayPOS.Database;
using SankayPOS.Models;

namespace SankayPOS.Forms;

public partial class TableManagementForm : Form
{
    private readonly TableRepository _tableRepo = new();
    private ListBox? _lstTables;
    private TextBox? _txtTableName;
    
    public TableManagementForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        LoadTables();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(600, 500);
        this.Name = "TableManagementForm";
        this.Text = "Masa Yönetimi";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        var lblTitle = new Label
        {
            Text = "Masa Yönetimi",
            Location = new Point(20, 20),
            Size = new Size(560, 30),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White
        };
        
        _lstTables = new ListBox
        {
            Location = new Point(20, 70),
            Size = new Size(300, 350),
            Font = new Font("Segoe UI", 10)
        };
        _lstTables.SelectedIndexChanged += LstTables_SelectedIndexChanged;
        
        var lblName = new Label
        {
            Text = "Masa Adı:",
            Location = new Point(340, 70),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtTableName = new TextBox
        {
            Location = new Point(340, 100),
            Size = new Size(240, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var btnAdd = new Button
        {
            Text = "Yeni Masa Ekle",
            Location = new Point(340, 140),
            Size = new Size(240, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAdd.Click += BtnAdd_Click;
        
        var btnUpdate = new Button
        {
            Text = "Masayı Güncelle",
            Location = new Point(340, 185),
            Size = new Size(240, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnUpdate.Click += BtnUpdate_Click;
        
        var btnDelete = new Button
        {
            Text = "Masayı Sil",
            Location = new Point(340, 230),
            Size = new Size(240, 35),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnDelete.Click += BtnDelete_Click;
        
        this.Controls.Add(lblTitle);
        this.Controls.Add(_lstTables);
        this.Controls.Add(lblName);
        this.Controls.Add(_txtTableName);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnUpdate);
        this.Controls.Add(btnDelete);
    }
    
    private void LoadTables()
    {
        _lstTables?.Items.Clear();
        var tables = _tableRepo.GetAllTables();
        
        foreach (var table in tables)
        {
            _lstTables?.Items.Add(new TableDisplay { Table = table });
        }
    }
    
    private void LstTables_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_lstTables?.SelectedItem is TableDisplay display && _txtTableName != null)
        {
            _txtTableName.Text = display.Table.Name;
        }
    }
    
    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtTableName?.Text))
        {
            MessageBox.Show("Masa adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        var tables = _tableRepo.GetAllTables();
        var nextPosition = tables.Count > 0 ? tables.Max(t => t.Position) + 1 : 1;
        
        _tableRepo.AddTable(_txtTableName.Text, nextPosition);
        LoadTables();
        
        if (_txtTableName != null)
            _txtTableName.Text = "";
        
        MessageBox.Show("Masa eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (_lstTables?.SelectedItem is not TableDisplay display)
        {
            MessageBox.Show("Lütfen bir masa seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(_txtTableName?.Text))
        {
            MessageBox.Show("Masa adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        _tableRepo.UpdateTable(display.Table.Id, _txtTableName.Text);
        LoadTables();
        MessageBox.Show("Masa güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_lstTables?.SelectedItem is not TableDisplay display)
        {
            MessageBox.Show("Lütfen bir masa seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var result = MessageBox.Show(
            $"{display.Table.Name} masasını silmek istediğinizden emin misiniz?",
            "Onay",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _tableRepo.DeleteTable(display.Table.Id);
            LoadTables();
            
            if (_txtTableName != null)
                _txtTableName.Text = "";
            
            MessageBox.Show("Masa silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
    private class TableDisplay
    {
        public Table Table { get; set; } = null!;
        public override string ToString() => Table.Name;
    }
}
