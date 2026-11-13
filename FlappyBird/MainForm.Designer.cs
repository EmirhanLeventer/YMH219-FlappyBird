namespace FlappyBird;

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
        components = new System.ComponentModel.Container();
        gameTimer = new System.Windows.Forms.Timer(components);
        gamePanel = new GamePanel();
        infoPanel = new Panel();
        lblScore = new Label();
        lblSpeed = new Label();
        lblState = new Label();
        btnNewGame = new Button();
        infoPanel.SuspendLayout();
        SuspendLayout();
        //
        // infoPanel
        //
        infoPanel.BackColor = Color.FromArgb(40, 44, 52);
        infoPanel.Controls.Add(lblScore);
        infoPanel.Controls.Add(lblSpeed);
        infoPanel.Controls.Add(lblState);
        infoPanel.Controls.Add(btnNewGame);
        infoPanel.Dock = DockStyle.Top;
        infoPanel.Location = new Point(0, 0);
        infoPanel.Name = "infoPanel";
        infoPanel.Size = new Size(480, 44);
        infoPanel.TabIndex = 1;
        //
        // lblScore
        //
        lblScore.AutoSize = true;
        lblScore.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblScore.ForeColor = Color.White;
        lblScore.Location = new Point(10, 12);
        lblScore.Name = "lblScore";
        lblScore.Text = "Skor: 0";
        //
        // lblSpeed
        //
        lblSpeed.AutoSize = true;
        lblSpeed.Font = new Font("Segoe UI", 11F);
        lblSpeed.ForeColor = Color.Gainsboro;
        lblSpeed.Location = new Point(100, 12);
        lblSpeed.Name = "lblSpeed";
        lblSpeed.Text = "Hız: 160";
        //
        // lblState
        //
        lblState.AutoSize = true;
        lblState.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblState.ForeColor = Color.Gold;
        lblState.Location = new Point(190, 12);
        lblState.Name = "lblState";
        lblState.Text = "● HAZIR";
        //
        // btnNewGame
        //
        btnNewGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNewGame.BackColor = Color.FromArgb(76, 175, 80);
        btnNewGame.Cursor = Cursors.Hand;
        btnNewGame.FlatAppearance.BorderSize = 0;
        btnNewGame.FlatStyle = FlatStyle.Flat;
        btnNewGame.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnNewGame.ForeColor = Color.White;
        btnNewGame.Location = new Point(366, 7);
        btnNewGame.Name = "btnNewGame";
        btnNewGame.Size = new Size(106, 30);
        btnNewGame.TabStop = false;
        btnNewGame.Text = "Yeni Oyun";
        btnNewGame.UseVisualStyleBackColor = false;
        btnNewGame.Click += btnNewGame_Click;
        //
        // gameTimer
        //
        gameTimer.Interval = 15;
        gameTimer.Tick += gameTimer_Tick;
        //
        // gamePanel
        //
        gamePanel.Dock = DockStyle.Fill;
        gamePanel.Location = new Point(0, 44);
        gamePanel.Name = "gamePanel";
        gamePanel.Size = new Size(480, 640);
        gamePanel.TabIndex = 0;
        gamePanel.Paint += gamePanel_Paint;
        gamePanel.MouseDown += gamePanel_MouseDown;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(480, 684);
        Controls.Add(gamePanel);
        Controls.Add(infoPanel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Flappy Bird - YMH219";
        KeyDown += MainForm_KeyDown;
        Load += MainForm_Load;
        infoPanel.ResumeLayout(false);
        infoPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Timer gameTimer;
    private GamePanel gamePanel;
    private Panel infoPanel;
    private Label lblScore;
    private Label lblSpeed;
    private Label lblState;
    private Button btnNewGame;
}
