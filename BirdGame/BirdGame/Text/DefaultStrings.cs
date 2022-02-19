namespace BirdGame.Text
{
    using System.Collections.Generic;

    /// <summary>
    /// A list of default strings for use in the game.
    /// </summary>
    internal class DefaultStrings
    {
        /// <summary>
        /// A dictionary of strings, with an id and the text they display.
        /// </summary>
        public static Dictionary<string, string> Strings = new Dictionary<string, string>
        {
            { "GameTitle", "Bird Game"},
            { "NewScore", "New High Score"},
            { "PersonalBest", "Personal Best: {0}"},
            { "SoundEffects", "Sound Effects"},
            { "Resolution", "Resolution"},
            { "Fullscreen", "Fullscreen"},
            { "Yes", "Yes"},
            { "No", "No"},
            { "Quit", "Quit Game" },
            { "PoopHead", "Shampoo"},
            { "PoopJacket", "Designer Label"},
            { "PoopCoffee", "Crapuccino"},
            { "PoopCar", "Mini Pooper"},
            { "PoopIceCream", "Extra Sprinkles"},
            { "PoopChips", "Side of Mayo"},
            { "DiveCoffee", "Stay Hydrated"},
            { "DiveIceCream", "Swoop and Scoop"},
            { "DiveChips", "Chippy Tea"},
        };
    }
}
