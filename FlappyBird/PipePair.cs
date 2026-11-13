namespace FlappyBird
{
    /// <summary>
    /// Aralarında kuşun geçebileceği boşluk bulunan bir üst ve bir alt borudan oluşan engel.
    /// Sağdan sola hareket eder; ekrandan çıkınca oyun motoru tarafından yeniden konumlandırılır.
    /// </summary>
    public class PipePair
    {
        public const float Width = 70f;

        /// <summary>Borunun ağız kısmının (kapak) yüksekliği ve yanlara taşma miktarı.</summary>
        private const float CapHeight = 22f;
        private const float CapOverhang = 4f;

        /// <summary>Alt borunun bittiği yer (zeminin üst kenarı).</summary>
        private readonly float _groundY;

        /// <summary>Borunun sol kenarının yatay konumu.</summary>
        public float X { get; private set; }

        /// <summary>Boşluğun üst kenarı (üst borunun bittiği yer).</summary>
        public float GapY { get; private set; }

        /// <summary>Üst ve alt boru arasındaki boşluğun yüksekliği.</summary>
        public float GapHeight { get; }

        /// <summary>Kuş bu engeli geçti mi? Skorun aynı engel için iki kez artmasını önler.</summary>
        public bool Passed { get; private set; }

        public float Right => X + Width;

        /// <summary>Borunun tamamı ekranın solundan dışarı çıktı mı?</summary>
        public bool IsOffScreen => Right < 0f;

        public RectangleF TopRect => new RectangleF(X, 0f, Width, GapY);

        public RectangleF BottomRect
        {
            get
            {
                float bottomTop = GapY + GapHeight;
                return new RectangleF(X, bottomTop, Width, _groundY - bottomTop);
            }
        }

        public PipePair(float x, float gapY, float gapHeight, float groundY)
        {
            GapHeight = gapHeight;
            _groundY = groundY;
            Reposition(x, gapY);
        }

        /// <summary>Boruyu verilen hız ve süreye göre sola kaydırır.</summary>
        public void Move(float speed, float deltaSeconds)
        {
            X -= speed * deltaSeconds;
        }

        /// <summary>Boruyu yeni bir konuma taşır ve "geçildi" bilgisini sıfırlar.</summary>
        public void Reposition(float x, float gapY)
        {
            X = x;
            GapY = gapY;
            Passed = false;
        }

        /// <summary>Engelin geçildiğini işaretler.</summary>
        public void MarkPassed()
        {
            Passed = true;
        }

        /// <summary>Verilen dikdörtgen üst veya alt boruyla kesişiyor mu?</summary>
        public bool CollidesWith(RectangleF rect)
        {
            return rect.IntersectsWith(TopRect) || rect.IntersectsWith(BottomRect);
        }

        public void Draw(Graphics g)
        {
            using var body = new SolidBrush(Color.FromArgb(90, 180, 60));
            using var cap = new SolidBrush(Color.FromArgb(70, 150, 45));
            using var outline = new Pen(Color.FromArgb(40, 90, 25), 2f);

            DrawPipe(g, TopRect, body, cap, outline, capAtBottom: true);
            DrawPipe(g, BottomRect, body, cap, outline, capAtBottom: false);
        }

        /// <summary>Tek bir boruyu gövde + ağız (kapak) olarak çizer.</summary>
        private static void DrawPipe(Graphics g, RectangleF rect, Brush body, Brush cap, Pen outline, bool capAtBottom)
        {
            if (rect.Height <= 0f)
                return;

            g.FillRectangle(body, rect);
            g.DrawRectangle(outline, rect.X, rect.Y, rect.Width, rect.Height);

            float capY = capAtBottom ? rect.Bottom - CapHeight : rect.Top;
            var capRect = new RectangleF(rect.X - CapOverhang, capY, rect.Width + CapOverhang * 2f, CapHeight);
            g.FillRectangle(cap, capRect);
            g.DrawRectangle(outline, capRect.X, capRect.Y, capRect.Width, capRect.Height);
        }
    }
}
