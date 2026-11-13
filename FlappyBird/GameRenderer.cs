using System.Drawing.Drawing2D;

namespace FlappyBird
{
    /// <summary>
    /// Oyun ekranını çizer: gökyüzü, borular, zemin, kuş ve ekran yazıları.
    /// Oyun mantığına dokunmaz; GameEngine'deki değerleri yalnızca okur.
    /// </summary>
    public class GameRenderer
    {
        private const float StripeWidth = 24f; // Zemin desenindeki şerit genişliği

        private readonly Font _scoreFont = new Font("Segoe UI", 36f, FontStyle.Bold);
        private readonly Font _messageFont = new Font("Segoe UI", 16f, FontStyle.Bold);
        private readonly Font _smallFont = new Font("Segoe UI", 12f, FontStyle.Bold);

        public void Draw(Graphics g, GameEngine game)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawSky(g);

            foreach (var pipe in game.Pipes)
                pipe.Draw(g);

            DrawGround(g, game.GroundOffset);
            game.Bird.Draw(g);

            DrawCenteredText(g, game.Score.ToString(), _scoreFont, 30f);

            if (game.State == GameState.Ready)
                DrawCenteredText(g, "Başlamak için SPACE / ↑ tuşuna bas", _messageFont, 200f);
            else if (game.State == GameState.GameOver)
                DrawGameOver(g, game);
        }

        /// <summary>Oyun bitince ekranın ortasına yarı saydam bir bilgi kutusu çizer.</summary>
        private void DrawGameOver(Graphics g, GameEngine game)
        {
            var box = new RectangleF(60f, 190f, GameEngine.FieldWidth - 120f, 190f);

            using (var background = new SolidBrush(Color.FromArgb(190, 30, 30, 40)))
                g.FillRectangle(background, box);
            using (var border = new Pen(Color.White, 2f))
                g.DrawRectangle(border, box.X, box.Y, box.Width, box.Height);

            DrawCenteredText(g, "GAME OVER", _scoreFont, box.Y + 15f);
            DrawCenteredText(g, $"Skor: {game.Score}", _messageFont, box.Y + 85f);
            DrawCenteredText(g, "Yeniden: R / Enter ya da Yeni Oyun", _smallFont, box.Y + 135f);
        }

        private static void DrawSky(Graphics g)
        {
            var skyRect = new RectangleF(0f, 0f, GameEngine.FieldWidth, GameEngine.GroundY);
            using var sky = new LinearGradientBrush(skyRect, Color.FromArgb(110, 195, 235),
                                                    Color.FromArgb(200, 235, 250), LinearGradientMode.Vertical);
            g.FillRectangle(sky, skyRect);
        }

        /// <summary>Zemini çizer; şeritler GroundOffset kadar kaydırılarak hareket hissi verilir.</summary>
        private static void DrawGround(Graphics g, float offset)
        {
            float y = GameEngine.GroundY;

            using var dirt = new SolidBrush(Color.FromArgb(222, 205, 140));
            using var grass = new SolidBrush(Color.FromArgb(110, 190, 70));
            using var stripe = new SolidBrush(Color.FromArgb(90, 165, 55));

            g.FillRectangle(dirt, 0f, y, GameEngine.FieldWidth, GameEngine.GroundHeight);
            g.FillRectangle(grass, 0f, y, GameEngine.FieldWidth, 14f);

            for (float x = -offset; x < GameEngine.FieldWidth; x += StripeWidth * 2f)
                g.FillRectangle(stripe, x, y, StripeWidth, 14f);

            g.DrawLine(Pens.DarkOliveGreen, 0f, y, GameEngine.FieldWidth, y);
        }

        /// <summary>Metni yatayda ortalar; okunabilmesi için arkasına koyu gölge çizer.</summary>
        private static void DrawCenteredText(Graphics g, string text, Font font, float y)
        {
            SizeF size = g.MeasureString(text, font);
            float x = (GameEngine.FieldWidth - size.Width) / 2f;

            using var shadow = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
            g.DrawString(text, font, shadow, x + 2f, y + 2f);
            g.DrawString(text, font, Brushes.White, x, y);
        }
    }
}
