namespace FlappyBird
{
    /// <summary>
    /// Oyunun çizildiği yüzey. Normal Panel'den farkları:
    /// - Çift tamponlama (DoubleBuffered): her karede ekranın titremesini önler.
    /// - Odak alabilir (Selectable): klavye olayları bu panele gelir.
    /// </summary>
    public class GamePanel : Panel
    {
        public GamePanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            TabStop = true;
        }

        /// <summary>
        /// Ok tuşları normalde odağı başka kontrole taşımak için kullanılır.
        /// Burada "girdi tuşu" sayıyoruz ki KeyDown olayına ulaşsınlar.
        /// </summary>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
                return true;

            return base.IsInputKey(keyData);
        }

        /// <summary>Panele tıklanınca odağı alsın (klavye kontrolü kaybolmasın).</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }
    }
}
