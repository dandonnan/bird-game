namespace BirdGame.Data
{
    /// <summary>
    /// The game's saved data.
    /// </summary>
    internal class SaveData
    {
        /// <summary>
        /// The minimum volume for sounds.
        /// </summary>
        public const int MinVolume = 0;

        /// <summary>
        /// The maximum volume for sounds.
        /// </summary>
        public const int MaxVolume = 10;

        /// <summary>
        /// The player's highest score.
        /// </summary>
        public int HighScore { get; set; }

        /// <summary>
        /// The volume for sound effects.
        /// </summary>
        public int SoundEffectsVolume { get; set; }

        /// <summary>
        /// The game's resolution.
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Whether or not the game is fullscreen.
        /// </summary>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// The game's language. This is written out to the save file
        /// but is not used.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The constructor which sets values to the default.
        /// </summary>
        public SaveData()
        {
            HighScore = 0;
            SoundEffectsVolume = 7;
            Resolution = $"640x480";
            Fullscreen = false;
            Language = "English";
        }

        /// <summary>
        /// Get the width of the resolution by getting the first number from the resolution string.
        /// </summary>
        public int ResolutionWidth => int.Parse(Resolution.Substring(0, Resolution.IndexOf("x")));

        /// <summary>
        /// Get the height of the resolution by getting the second number from the resolution string.
        /// </summary>
        public int ResolutionHeight => int.Parse(Resolution.Substring(Resolution.IndexOf("x") + 1));
    }
}
