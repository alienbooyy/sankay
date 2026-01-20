using SankayPOS.Database;

namespace SankayPOS.Forms;

public partial class SplitPaymentForm : Form
{
    private readonly Models.Order _order;
    private TextBox? _txtAmount;
    
    public SplitPaymentForm(Models.Order order)
    {
        _order = order;
        InitializeComponent();
        InitializeCustomComponents();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        this.ClientSize = new Size(400, 300);
        this.Name = "SplitPaymentForm";
        this.Text = "Alman Usulü Ödeme";
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        
        this.ResumeLayout(false);
    }
    
    private void InitializeCustomComponents()
    {
        var lblTotal = new Label
        {
            Text = $"Toplam Tutar: {_order.TotalAmount:C}",
            Location = new Point(20, 20),
            Size = new Size(360, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };
        
        var lblAmount = new Label
        {
            Text = "Ödenecek Tutar:",
            Location = new Point(20, 70),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        _txtAmount = new TextBox
        {
            Location = new Point(180, 70),
            Size = new Size(200, 25),
            Font = new Font("Segoe UI", 10)
        };
        
        var btnPay = new Button
        {
            Text = "Ödeme Al",
            Location = new Point(100, 120),
            Size = new Size(200, 40),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnPay.Click += BtnPay_Click;
        
        var lblInfo = new Label
        {
            Text = "Alman usulü ödeme: Her kişi kendi\npayını öder. Kalan tutarı hesaplamak\niçin bu özelliği kullanın.",
            Location = new Point(20, 180),
            Size = new Size(360, 80),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Gray
        };
        
        this.Controls.Add(lblTotal);
        this.Controls.Add(lblAmount);
        this.Controls.Add(_txtAmount);
        this.Controls.Add(btnPay);
        this.Controls.Add(lblInfo);
    }
    
    private void BtnPay_Click(object? sender, EventArgs e)
    {
        if (!decimal.TryParse(_txtAmount?.Text, out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Geçerli bir tutar girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (amount > _order.TotalAmount)
        {
            MessageBox.Show("Girilen tutar toplam tutardan büyük olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        var remaining = _order.TotalAmount - amount;
        MessageBox.Show(
            $"Ödenen: {amount:C}\nKalan: {remaining:C}",
            "Ödeme Bilgisi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        
        if (remaining <= 0)
        {
            var orderRepo = new OrderRepository();
            orderRepo.CloseOrder(_order.Id);
            this.Close();
        }
    }
}
