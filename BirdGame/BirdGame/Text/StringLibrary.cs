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

        private Dictionary<string, string> PopulateDictionary()
        {
            return DefaultStrings.Strings.ToDictionary(d => d.Key, d => d.Value);
        }
    }
}
