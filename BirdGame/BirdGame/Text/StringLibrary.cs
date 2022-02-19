namespace BirdGame.Text
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A library of strings.
    /// </summary>
    internal class StringLibrary
    {
        /// <summary>
        /// A singleton for the string library, so only one can be active.
        /// </summary>
        private static StringLibrary stringLibrary;

        /// <summary>
        /// A dictionary of strings.
        /// </summary>
        private readonly Dictionary<string, string> stringDictionary;

        /// <summary>
        /// A private constructor, so the string library can only be created
        /// throughout the Initialise method.
        /// </summary>
        private StringLibrary()
        {
            stringDictionary = PopulateDictionary();

            stringLibrary = this;
        }

        /// <summary>
        /// Initialise the string library.
        /// </summary>
        public static void Initialise()
        {
            if (stringLibrary == null)
            {
                new StringLibrary();
            }
        }

        /// <summary>
        /// Get a string from the library.
        /// </summary>
        /// <param name="id">The id of the string.</param>
        /// <returns>The string if found, null if not.</returns>
        public static string GetString(string id)
        {
            // If a string with the id is not in the dictionary
            if (stringLibrary.stringDictionary.TryGetValue(id, out string value) == false)
            {
                // Get the string with the id from the default list
                // This is never needed since the game populates from the default list
                // anyway, but if other languages were implemented it would use the
                // list as a backup if a string was not translated
                DefaultStrings.Strings.TryGetValue(id, out value);
            }

            return value;
        }

        /// <summary>
        /// Populate the dictionary.
        /// </summary>
        /// <returns>A dictionary of strings.</returns>
        private Dictionary<string, string> PopulateDictionary()
        {
            // Get the strings from the default strings list
            return DefaultStrings.Strings.ToDictionary(d => d.Key, d => d.Value);
        }
    }
}
