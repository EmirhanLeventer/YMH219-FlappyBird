namespace FlappyBird
{
    /// <summary>
    /// Oyuncunun kontrol ettiği kuş. Konumunu, dikey hızını ve yer çekimi etkisini yönetir.
    /// Birimler: konum piksel, hız piksel/saniye, ivme piksel/saniye².
    /// </summary>
    public class Bird
    {
        public const float Width = 34f;
        public const float Height = 24f;

        /// <summary>Her saniye aşağı doğru hıza eklenen değer (yer çekimi ivmesi).</summary>
        public const float Gravity = 1500f;

        /// <summary>Zıplayınca kuşa verilen yukarı yönlü hız.</summary>
        public const float FlapVelocity = -450f;

        /// <summary>Kuşun ulaşabileceği en yüksek düşüş hızı.</summary>
        public const float MaxFallSpeed = 700f;

        private readonly float _startY;

        /// <summary>Yatay konum sabittir; kuş değil engeller hareket eder.</summary>
        public float X { get; }

        /// <summary>Kuşun üst kenarının dikey konumu. Dışarıdan yalnızca okunabilir.</summary>
        public float Y { get; private set; }

        /// <summary>Dikey hız: negatif = yukarı, pozitif = aşağı.</summary>
        public float VelocityY { get; private set; }

        /// <summary>Çarpışma kontrolünde kullanılan dikdörtgen.</summary>
        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        public Bird(float x, float startY)
        {
            X = x;
            _startY = startY;
            Reset();
        }

        /// <summary>Kuşu başlangıç konumuna ve sıfır hıza döndürür.</summary>
        public void Reset()
        {
            Y = _startY;
            VelocityY = 0f;
        }

        /// <summary>Kuşu yukarı zıplatır: hızı anında yukarı yönlü sabit değere ayarlar.</summary>
        public void Flap()
        {
            VelocityY = FlapVelocity;
        }

        /// <summary>
        /// Geçen süreye göre kuşu hareket ettirir.
        /// Önce yer çekimi hızı değiştirir, sonra hız konumu değiştirir.
        /// </summary>
        /// <param name="deltaSeconds">Önceki güncellemeden bu yana geçen süre (saniye).</param>
        public void Update(float deltaSeconds)
        {
            VelocityY = Math.Min(VelocityY + Gravity * deltaSeconds, MaxFallSpeed);
            Y += VelocityY * deltaSeconds;

            // Ekranın üstünden dışarı çıkmasın; tavana değerse yukarı hızı sıfırlanır.
            if (Y < 0f)
            {
                Y = 0f;
                VelocityY = 0f;
            }
        }

        /// <summary>Kuşu çizer. Hıza göre burnunu yukarı/aşağı eğer.</summary>
        public void Draw(Graphics g)
        {
            // Hız -450..700 aralığında, açı yaklaşık -25..+60 derece olacak şekilde ölçeklenir.
            float angle = Math.Clamp(VelocityY / 10f, -25f, 60f);

            var state = g.Save();
            g.TranslateTransform(X + Width / 2f, Y + Height / 2f); // kuşun merkezine taşı
            g.RotateTransform(angle);

            float left = -Width / 2f;
            float top = -Height / 2f;

            using (var body = new SolidBrush(Color.Gold))
            using (var outline = new Pen(Color.FromArgb(120, 80, 0), 2f))
            {
                g.FillEllipse(body, left, top, Width, Height);
                g.DrawEllipse(outline, left, top, Width, Height);
            }

            // Kanat
            using (var wing = new SolidBrush(Color.Khaki))
                g.FillEllipse(wing, left + 4f, top + 10f, 14f, 9f);

            // Göz
            g.FillEllipse(Brushes.White, left + 20f, top + 3f, 10f, 10f);
            g.FillEllipse(Brushes.Black, left + 25f, top + 6f, 4f, 4f);

            // Gaga
            PointF[] beak =
            {
                new PointF(left + Width - 4f, top + 11f),
                new PointF(left + Width + 8f, top + 14f),
                new PointF(left + Width - 4f, top + 18f)
            };
            g.FillPolygon(Brushes.OrangeRed, beak);

            g.Restore(state);
        }
    }
}
