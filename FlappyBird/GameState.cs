namespace FlappyBird
{
    /// <summary>Oyunun içinde bulunabileceği durumlar.</summary>
    public enum GameState
    {
        /// <summary>Oyun hazır, oyuncunun ilk tuşa basması bekleniyor.</summary>
        Ready,

        /// <summary>Oyun oynanıyor.</summary>
        Playing,

        /// <summary>Kuş bir engele veya zemine çarptı, oyun durdu.</summary>
        GameOver
    }
}
