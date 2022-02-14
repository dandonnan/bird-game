namespace BirdGame.Data
{
    using System.IO;

    internal class SaveData
    {
        private const string Filename = "wings";

        public int HighScore;

        public static void Save(SaveData data)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(Filename))
                {
                    streamWriter.Write(data.HighScore);
                }
            }
            catch
            {
                // Don't bother exception handling, it's only a small game
                // and this is likely to be an edge-case anyway
            }
        }

        public static SaveData Load()
        {
            SaveData saveData = new SaveData();

            if (File.Exists(Filename))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(Filename))
                    {
                        string firstLine = streamReader.ReadLine();

                        if (int.TryParse(firstLine, out int score))
                        {
                            saveData.HighScore = score;
                        }
                    }
                }
                catch
                {
                    // Don't bother exception handling, it's only a small game
                    // and this is likely to be an edge-case anyway
                }
            }

            return saveData;
        }
    }
}
