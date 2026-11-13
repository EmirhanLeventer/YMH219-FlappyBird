using System.Diagnostics;

namespace FlappyBird;

/// <summary>
/// Ana pencere. Yalnızca arayüz işleri yapar:
/// Timer ile oyun döngüsünü çalıştırır, klavye olaylarını oyuna iletir ve ekranı çizdirir.
/// Oyun kuralları GameEngine sınıfındadır.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>Bilgisayar takılırsa kuşun "ışınlanmaması" için tek adımdaki en büyük süre.</summary>
    private const float MaxDeltaSeconds = 0.05f;

    private readonly GameEngine _game = new GameEngine();
    private readonly GameRenderer _renderer = new GameRenderer();

    /// <summary>İki Tick arasında geçen gerçek süreyi ölçer.</summary>
    private readonly Stopwatch _stopwatch = new Stopwatch();

    public MainForm()
    {
        InitializeComponent();
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        _stopwatch.Start();
        gameTimer.Start();
        gamePanel.Focus();
    }

    /// <summary>
    /// Oyun döngüsü: Timer her tetiklendiğinde geçen süre ölçülür,
    /// oyun bu süre kadar ilerletilir ve ekran yeniden çizdirilir.
    /// </summary>
    private void gameTimer_Tick(object? sender, EventArgs e)
    {
        float deltaSeconds = (float)_stopwatch.Elapsed.TotalSeconds;
        _stopwatch.Restart();

        _game.Update(Math.Min(deltaSeconds, MaxDeltaSeconds));
        gamePanel.Invalidate(); // Paint olayını tetikler
    }

    private void gamePanel_Paint(object? sender, PaintEventArgs e)
    {
        // Ekran ölçeği (DPI) farklıysa çizimi panel boyutuna göre büyüt/küçült.
        e.Graphics.ScaleTransform(gamePanel.ClientSize.Width / (float)GameEngine.FieldWidth,
                                  gamePanel.ClientSize.Height / (float)GameEngine.FieldHeight);
        _renderer.Draw(e.Graphics, _game);
    }

    /// <summary>
    /// KeyPreview = true olduğu için hangi kontrol odakta olursa olsun tuşlar önce buraya gelir.
    /// </summary>
    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            _game.Flap();
            e.Handled = true;
            e.SuppressKeyPress = true; // Tuş başka kontrole (ör. butona) ulaşmasın.
        }
    }

    private void gamePanel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            _game.Flap();
    }
}
