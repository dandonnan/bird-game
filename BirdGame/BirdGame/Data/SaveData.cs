namespace BirdGame.Data
{
    internal class SaveData
    {
        public const int MinVolume = 0;

        public const int MaxVolume = 10;

        public int HighScore { get; set; }

        public int SoundEffectsVolume { get; set; }

        public string Resolution { get; set; }

        public bool Fullscreen { get; set; }

        public string Language { get; set; }

        public SaveData()
        {
            HighScore = 0;
            SoundEffectsVolume = 7;
            Resolution = $"{MainGame.DefaultWidth}x{MainGame.DefaultHeight}";
            Fullscreen = false;
            Language = "English";
        }

        public int ResolutionWidth => int.Parse(Resolution.Substring(0, Resolution.IndexOf("x")));

        public int ResolutionHeight => int.Parse(Resolution.Substring(Resolution.IndexOf("x") + 1));
    }
}
