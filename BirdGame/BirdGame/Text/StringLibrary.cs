namespace BirdGame.Text
{
    using System.Collections.Generic;
    using System.Linq;

    internal class StringLibrary
    {
        private static StringLibrary stringLibrary;

        private Dictionary<string, string> stringDictionary;

        private StringLibrary()
        {
            stringDictionary = PopulateDictionary();

            stringLibrary = this;
        }

        public static void Initialise()
        {
            if (stringLibrary == null)
            {
                new StringLibrary();
            }
        }

        public static string GetString(string id)
        {
            if (stringLibrary.stringDictionary.TryGetValue(id, out string value) == false)
            {
                DefaultStrings.Strings.TryGetValue(id, out value);
            }

            return value;
        }

        public static void ChangeLanguage(string language)
        {
            // TODO: change
            stringLibrary.stringDictionary = new Dictionary<string, string>();
        }

        private Dictionary<string, string> PopulateDictionary()
        {
            // TODO: populate from file
            return DefaultStrings.Strings.ToDictionary(d => d.Key, d => d.Value);
        }
    }
}
