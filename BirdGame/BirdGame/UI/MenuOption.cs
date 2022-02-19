namespace BirdGame.UI
{
    using BirdGame.Input;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A menu option.
    /// </summary>
    internal class MenuOption
    {
        /// <summary>
        /// A method to call when the option has changed.
        /// </summary>
        private readonly Action<int> onOptionChanged;

        /// <summary>
        /// The title of the option.
        /// </summary>
        private readonly string title;

        /// <summary>
        /// The font to draw the title with.
        /// </summary>
        private readonly SpriteFont font;

        /// <summary>
        /// The list of options.
        /// </summary>
        private readonly List<string> options;

        /// <summary>
        /// The position to draw the option title on the left.
        /// </summary>
        private readonly Vector2 leftPosition;

        /// <summary>
        /// The position to draw the option on the right.
        /// </summary>
        private readonly Vector2 rightPosition;

        /// <summary>
        /// The currently selected option.
        /// </summary>
        private int currentOption;

        /// <summary>
        /// The constructor for the menu option.
        /// </summary>
        /// <param name="title">The title of the option.</param>
        /// <param name="options">A list of options.</param>
        /// <param name="currentOption">The currently selected option.</param>
        /// <param name="yOffset">The offset from the top of the screen.</param>
        /// <param name="onOptionChanged">The method to call when the option has changed.</param>
        public MenuOption(string title, List<string> options, int currentOption,
                            int yOffset, Action<int> onOptionChanged)
        {
            this.title = title;
            this.options = options;
            this.currentOption = currentOption;
            this.onOptionChanged = onOptionChanged;

            // Load the font
            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            // If there is a list of options
            if (options != null)
            {
                int screenMiddle = MainGame.DefaultWidth / 2;

                // Set the left position to be on the left of the screen's center
                leftPosition = new Vector2(screenMiddle - font.MeasureString(title).X, yOffset);

                // Set the right position to be on the right of the screen's center
                rightPosition = new Vector2(screenMiddle + 10, yOffset);
            }
            // If there is not a list of options
            else
            {
                // Display the left option in the middle
                leftPosition = new Vector2((MainGame.DefaultWidth - font.MeasureString(title).X) / 2, yOffset);
            }
        }

        /// <summary>
        /// Update the option.
        /// </summary>
        public void Update()
        {
            // If the left button is pressed and there is a method when the option has changed
            if (InputManager.IsBindingPressed(DefaultBindings.Left) && onOptionChanged != null)
            {
                // Set the option to the previous one
                currentOption--;

                // If the current option is below 0 (the first one)
                if (currentOption < 0)
                {
                    // Wrap the current option to the last one
                    currentOption = options.Count - 1;
                }

                // Call the option changed method
                onOptionChanged(currentOption);
            }

            // If the right button is pressed and there is a method when the option has changed
            if (InputManager.IsBindingPressed(DefaultBindings.Right) && onOptionChanged != null)
            {
                // Set the option to the next one
                currentOption++;

                // If the current option is above the number of options (the last one)
                if (currentOption >= options.Count)
                {
                    // Wrap the current option to the first one
                    currentOption = 0;
                }

                // Call the option changed method
                onOptionChanged(currentOption);
            }
        }

        /// <summary>
        /// Draw the option.
        /// </summary>
        public void Draw()
        {
            // Draw the option
            WorldManager.SpriteBatch.DrawString(font, title, leftPosition, Color.White);

            // If there is a list of options
            if (options != null)
            {
                // Draw the current option on the right
                WorldManager.SpriteBatch.DrawString(font, options[currentOption], rightPosition, Color.White);
            }
        }

        /// <summary>
        /// Draw the option when it is highlighted.
        /// </summary>
        public void DrawHighlighted()
        {
            // Draw the option
            WorldManager.SpriteBatch.DrawString(font, title, leftPosition, Color.GreenYellow);

            // If there is a list of options
            if (options != null)
            {
                // Draw the current option on the right
                WorldManager.SpriteBatch.DrawString(font, options[currentOption], rightPosition, Color.GreenYellow);
            }
        }
    }
}
