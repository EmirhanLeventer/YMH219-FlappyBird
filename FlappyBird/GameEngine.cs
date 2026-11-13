namespace FlappyBird
{
    /// <summary>
    /// Oyunun tüm kurallarını yönetir: durum, skor, hız, kuş, borular ve çarpışma kontrolü.
    /// Arayüzden (Form) bağımsızdır; değişiklikleri event'ler ile dışarıya bildirir.
    /// </summary>
    public class GameEngine
    {
        // Oyun alanının boyutları (piksel).
        public const int FieldWidth = 480;
        public const int FieldHeight = 640;
        public const int GroundHeight = 80;
        public const float GroundY = FieldHeight - GroundHeight;

        // Boru ayarları.
        private const int PipeCount = 3;            // Ekranda dönen boru sayısı (föy: en az 2)
        private const float PipeSpacing = 240f;     // İki boru arasındaki yatay mesafe
        private const float PipeGapHeight = 160f;   // Kuşun geçeceği boşluk
        private const float MinGapMargin = 60f;     // Boşluğun üstten/alttan en az uzaklığı
        private const float FirstPipeX = FieldWidth + 100f;

        /// <summary>Oyun başladığında boruların sola kayma hızı (piksel/saniye).</summary>
        public const float BaseSpeed = 160f;

        private readonly Random _random = new Random();
        private readonly List<PipePair> _pipes = new List<PipePair>();

        public Bird Bird { get; }

        /// <summary>Boruların salt okunur listesi (dışarıdan eklenip silinemez).</summary>
        public IReadOnlyList<PipePair> Pipes => _pipes;

        public GameState State { get; private set; }

        public int Score { get; private set; }

        /// <summary>Boruların ve zeminin anlık kayma hızı (piksel/saniye).</summary>
        public float Speed { get; private set; }

        /// <summary>Zemin desenini kaydırmak için kullanılan değer (yalnızca görsel).</summary>
        public float GroundOffset { get; private set; }

        /// <summary>Skor değiştiğinde tetiklenir.</summary>
        public event EventHandler? ScoreChanged;

        /// <summary>Oyun durumu (Ready / Playing / GameOver) değiştiğinde tetiklenir.</summary>
        public event EventHandler? StateChanged;

        public GameEngine()
        {
            Bird = new Bird(100f, (GroundY - Bird.Height) / 2f);

            for (int i = 0; i < PipeCount; i++)
                _pipes.Add(new PipePair(0f, 0f, PipeGapHeight, GroundY));

            Reset();
        }

        /// <summary>Yeni oyun: her şeyi başlangıç değerlerine döndürür ve Ready durumuna geçer.</summary>
        public void Reset()
        {
            Bird.Reset();
            Score = 0;
            Speed = BaseSpeed;
            GroundOffset = 0f;

            // Borular ekranın sağından başlayarak eşit aralıklarla dizilir.
            for (int i = 0; i < _pipes.Count; i++)
                _pipes[i].Reposition(FirstPipeX + i * PipeSpacing, RandomGapY());

            ScoreChanged?.Invoke(this, EventArgs.Empty);
            ChangeState(GameState.Ready);
        }

        /// <summary>
        /// Oyuncu zıplama tuşuna bastığında çağrılır.
        /// Ready durumundaysa oyunu başlatır; Game Over durumunda hiçbir şey yapmaz.
        /// </summary>
        public void Flap()
        {
            if (State == GameState.GameOver)
                return;

            if (State == GameState.Ready)
                ChangeState(GameState.Playing);

            Bird.Flap();
        }

        /// <summary>
        /// Oyun döngüsünün bir adımı (Timer her tetiklendiğinde çağrılır).
        /// </summary>
        /// <param name="deltaSeconds">Önceki adımdan bu yana geçen süre (saniye).</param>
        public void Update(float deltaSeconds)
        {
            if (State != GameState.Playing)
                return;

            Bird.Update(deltaSeconds);
            MovePipes(deltaSeconds);
            UpdateScore();

            GroundOffset = (GroundOffset + Speed * deltaSeconds) % 24f;

            if (CheckCollision())
                ChangeState(GameState.GameOver);
        }

        private void MovePipes(float deltaSeconds)
        {
            foreach (var pipe in _pipes)
            {
                pipe.Move(Speed, deltaSeconds);

                // Ekrandan çıkan boru en sağdaki borunun arkasına taşınır (yeniden kullanılır).
                if (pipe.IsOffScreen)
                {
                    float rightmostX = _pipes.Max(p => p.X);
                    pipe.Reposition(rightmostX + PipeSpacing, RandomGapY());
                }
            }
        }

        /// <summary>Kuşun tamamen geçtiği her boru için skoru bir artırır.</summary>
        private void UpdateScore()
        {
            foreach (var pipe in _pipes)
            {
                if (!pipe.Passed && pipe.Right < Bird.X)
                {
                    pipe.MarkPassed();
                    Score++;
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>Kuş zemine veya herhangi bir boruya değdi mi?</summary>
        private bool CheckCollision()
        {
            if (Bird.Bounds.Bottom >= GroundY)
            {
                Bird.LandOn(GroundY);
                return true;
            }

            // Kuş oval çizildiği için çarpışma kutusu biraz küçültülür (daha adil his).
            RectangleF hitBox = Bird.Bounds;
            hitBox.Inflate(-3f, -3f);

            return _pipes.Any(p => p.CollidesWith(hitBox));
        }

        /// <summary>Boşluğun üst kenarı için rastgele ama güvenli bir değer üretir.</summary>
        private float RandomGapY()
        {
            float min = MinGapMargin;
            float max = GroundY - PipeGapHeight - MinGapMargin;
            return min + (float)_random.NextDouble() * (max - min);
        }

        private void ChangeState(GameState newState)
        {
            if (State == newState)
                return;

            State = newState;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
