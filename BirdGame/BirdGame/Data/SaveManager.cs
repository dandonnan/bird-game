namespace BirdGame.Data
{
    using System.IO;

    internal class SaveManager
    {
        private const string Filename = "wings";

        private static SaveManager saveManager;

        private SaveData saveData;

        private SaveManager()
        {
            saveManager = this;

            Load();
        }

        public static void Initialise()
        {
            if (saveManager == null)
            {
                new SaveManager();
            }
        }

        public static SaveData SaveData => saveManager.saveData;

        public static void Save()
        {
            try
            {
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

        public static void Load()
        {
            if (File.Exists(Filename))
            {
                try
                {
                    SaveData saveData = new SaveData();

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
                saveManager.saveData = new SaveData();
            }
        }
    }
}
