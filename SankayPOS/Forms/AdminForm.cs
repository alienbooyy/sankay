using SankayPOS.Database;
using OfficeOpenXml;
using System.Data;

namespace SankayPOS.Forms;

public partial class AdminForm : Form
{
    private readonly OrderRepository _orderRepo = new();
    private DateTimePicker? _dtpStart;
    private DateTimePicker? _dtpEnd;
    private DataGridView? _dgvReport;
    private Label? _lblTotalRevenue;
    
    // Set license context once for the application
    static AdminForm()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    
    public AdminForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
        CheckAdminPassword();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(1000, 600);
        this.Name = "AdminForm";
        this.Text = "Yönetici Paneli";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        this.ResumeLayout(false);
    }
    
    private void CheckAdminPassword()
    {
        var password = Microsoft.VisualBasic.Interaction.InputBox(
            "Yönetici şifresini girin:",
            "Giriş",
            "",
            -1, -1);
        
        var storedPassword = DatabaseHelper.ExecuteScalar(
            "SELECT Value FROM Settings WHERE Key = 'AdminPassword'")?.ToString() ?? "1234";
        
        if (password != storedPassword)
        {
            MessageBox.Show("Hatalı şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
    }
    
    private void InitializeCustomComponents()
    {
        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.FromArgb(52, 73, 94)
        };
        
        var lblTitle = new Label
        {
            Text = "Günlük Raporlar",
            Location = new Point(20, 10),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.White
        };
        
        var lblStart = new Label
        {
            Text = "Başlangıç:",
            Location = new Point(20, 50),
            Size = new Size(80, 25),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10)
        };
        
        _dtpStart = new DateTimePicker
        {
            Location = new Point(110, 50),
            Size = new Size(200, 25),
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Today
        };
        
        var lblEnd = new Label
        {
            Text = "Bitiş:",
            Location = new Point(330, 50),
            Size = new Size(50, 25),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10)
        };
        
        _dtpEnd = new DateTimePicker
        {
            Location = new Point(390, 50),
            Size = new Size(200, 25),
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Today
        };
        
        var btnShow = new Button
        {
            Text = "Göster",
            Location = new Point(610, 48),
            Size = new Size(100, 30),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnShow.Click += BtnShow_Click;
        
        var btnExport = new Button
        {
            Text = "Excel'e Aktar",
            Location = new Point(720, 48),
            Size = new Size(120, 30),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnExport.Click += BtnExport_Click;
        
        var btnPrint = new Button
        {
            Text = "Yazdır",
            Location = new Point(850, 48),
            Size = new Size(120, 30),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(155, 89, 182),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnPrint.Click += BtnPrint_Click;
        
        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(lblStart);
        headerPanel.Controls.Add(_dtpStart);
        headerPanel.Controls.Add(lblEnd);
        headerPanel.Controls.Add(_dtpEnd);
        headerPanel.Controls.Add(btnShow);
        headerPanel.Controls.Add(btnExport);
        headerPanel.Controls.Add(btnPrint);
        
        _dgvReport = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None
        };
        
        var footerPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 50,
            BackColor = Color.FromArgb(52, 73, 94)
        };
        
        _lblTotalRevenue = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Toplam Gelir: 0.00 TL",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        
        footerPanel.Controls.Add(_lblTotalRevenue);
        
        this.Controls.Add(_dgvReport);
        this.Controls.Add(footerPanel);
        this.Controls.Add(headerPanel);
    }
    
    private void BtnShow_Click(object? sender, EventArgs e)
    {
        if (_dtpStart == null || _dtpEnd == null || _dgvReport == null)
            return;
        
        var startDate = _dtpStart.Value.Date;
        var endDate = _dtpEnd.Value.Date;
        
        var reportData = _orderRepo.GetProductSalesReport(startDate, endDate);
        _dgvReport.DataSource = reportData;
        
        // Calculate total revenue
        decimal totalRevenue = 0;
        foreach (DataRow row in reportData.Rows)
        {
            if (row["TotalRevenue"] != DBNull.Value)
                totalRevenue += Convert.ToDecimal(row["TotalRevenue"]);
        }
        
        if (_lblTotalRevenue != null)
            _lblTotalRevenue.Text = $"Toplam Gelir: {totalRevenue:C}";
    }
    
    private void BtnExport_Click(object? sender, EventArgs e)
    {
        if (_dgvReport?.DataSource is not DataTable dt || dt.Rows.Count == 0)
        {
            MessageBox.Show("Dışa aktarılacak veri yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Rapor");
        
        // Headers
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
        }
        
        // Data
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j];
            }
        }
        
        var saveDialog = new SaveFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            FileName = $"Rapor_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };
        
        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            var file = new FileInfo(saveDialog.FileName);
            package.SaveAs(file);
            MessageBox.Show("Rapor başarıyla dışa aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        if (_dgvReport?.DataSource is not DataTable dt || dt.Rows.Count == 0)
        {
            MessageBox.Show("Yazdırılacak veri yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        MessageBox.Show("Yazdırma özelliği geliştirilme aşamasında.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
