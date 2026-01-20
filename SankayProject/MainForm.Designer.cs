namespace SankayProject;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        welcomeLabel = new Label();
        clickMeButton = new Button();
        SuspendLayout();
        // 
        // welcomeLabel
        // 
        welcomeLabel.AutoSize = true;
        welcomeLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        welcomeLabel.Location = new Point(250, 100);
        welcomeLabel.Name = "welcomeLabel";
        welcomeLabel.Size = new Size(300, 32);
        welcomeLabel.TabIndex = 0;
        welcomeLabel.Text = "Welcome to the System!";
        // 
        // clickMeButton
        // 
        clickMeButton.Location = new Point(300, 200);
        clickMeButton.Name = "clickMeButton";
        clickMeButton.Size = new Size(200, 50);
        clickMeButton.TabIndex = 1;
        clickMeButton.Text = "Click Me";
        clickMeButton.UseVisualStyleBackColor = true;
        clickMeButton.Click += ClickMeButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(clickMeButton);
        Controls.Add(welcomeLabel);
        Name = "MainForm";
        Text = "Sankay System";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label welcomeLabel;
    private Button clickMeButton;
}
