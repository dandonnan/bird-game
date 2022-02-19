namespace BirdGame.Data
{
    using System.IO;

    /// <summary>
    /// A save manager.
    /// </summary>
    internal class SaveManager
    {
        /// <summary>
        /// The save data file.
        /// </summary>
        private const string Filename = "wings";

        /// <summary>
        /// A singleton for the save manager, so there is only ever one instance.
        /// </summary>
        private static SaveManager saveManager;

        /// <summary>
        /// The save data.
        /// </summary>
        private SaveData saveData;

        /// <summary>
        /// A private constructor, so the manager can only be created through
        /// the Initialise method.
        /// </summary>
        private SaveManager()
        {
            saveManager = this;

            // Load the data
            Load();
        }

        /// <summary>
        /// Initialise the save manager.
        /// </summary>
        public static void Initialise()
        {
            if (saveManager == null)
            {
                new SaveManager();
            }
        }

        /// <summary>
        /// Get the save data.
        /// </summary>
        public static SaveData SaveData => saveManager.saveData;

        /// <summary>
        /// Save the data.
        /// </summary>
        public static void Save()
        {
            try
            {
                // Open the file and write the data to it
                using (StreamWriter streamWriter = new StreamWriter(Filename))
                {
                    streamWriter.WriteLine(SaveData.HighScore);
                    streamWriter.WriteLine(SaveData.SoundEffectsVolume);
                    streamWriter.WriteLine(SaveData.Resolution);
                    streamWriter.WriteLine(SaveData.Fullscreen);
                    streamWriter.WriteLine(SaveData.Language);
                }
            }
            catch
            {
                // Don't bother exception handling, it's only a small game
                // and this is likely to be an edge-case anyway
            }
        }

        /// <summary>
        /// Load the data.
        /// </summary>
        public static void Load()
        {
            // If the file exists
            if (File.Exists(Filename))
            {
                try
                {
                    SaveData saveData = new SaveData();

                    // Open the file, read the data and set it against the save data
                    using (StreamReader streamReader = new StreamReader(Filename))
                    {
                        string firstLine = streamReader.ReadLine();
                        string secondLine = streamReader.ReadLine();
                        saveData.Resolution = streamReader.ReadLine();
                        string fourthLine = streamReader.ReadLine();
                        saveData.Language = streamReader.ReadLine();

                        if (int.TryParse(firstLine, out int score))
                        {
                            saveData.HighScore = score;
                        }

                        if (int.TryParse(secondLine, out int sound))
                        {
                            saveData.SoundEffectsVolume = sound;
                        }

                        if (bool.TryParse(fourthLine, out bool fullscreen))
                        {
                            saveData.Fullscreen = fullscreen;
                        }
                    }

                    saveManager.saveData = saveData;
                }
                catch
                {
                    // Don't bother exception handling, it's only a small game
                    // and this is likely to be an edge-case anyway
                }
            }
            else
            {
                // Default the save data
                saveManager.saveData = new SaveData();
            }
        }
    }
}
