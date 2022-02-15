namespace BirdGame.UI
{
    using BirdGame.Input;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;

    internal class MenuOption
    {
        private readonly Action<int> onOptionChanged;

        private readonly string title;

        private readonly SpriteFont font;

        private readonly List<string> options;

        private readonly Vector2 leftPosition;

        private readonly Vector2 rightPosition;

        private int currentOption;

        public MenuOption(string title, List<string> options, int currentOption,
                            int yOffset, Action<int> onOptionChanged)
        {
            this.title = title;
            this.options = options;
            this.currentOption = currentOption;
            this.onOptionChanged = onOptionChanged;

            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            if (options != null)
            {
                int screenMiddle = MainGame.DefaultWidth / 2;

                leftPosition = new Vector2(screenMiddle - font.MeasureString(title).X, yOffset);
                rightPosition = new Vector2(screenMiddle + 10, yOffset);
            }
            else
            {
                leftPosition = new Vector2((MainGame.DefaultWidth - font.MeasureString(title).X) / 2, yOffset);
            }
        }

        public void Update()
        {
            if (InputManager.IsBindingPressed(DefaultBindings.Left) && onOptionChanged != null)
            {
                currentOption--;

                if (currentOption < 0)
                {
                    currentOption = options.Count - 1;
                }

                onOptionChanged(currentOption);
            }

            if (InputManager.IsBindingPressed(DefaultBindings.Right) && onOptionChanged != null)
            {
                currentOption++;

                if (currentOption >= options.Count)
                {
                    currentOption = 0;
                }

                onOptionChanged(currentOption);
            }
        }

        public void Draw()
        {
            WorldManager.SpriteBatch.DrawString(font, title, leftPosition, Color.White);

            if (options != null)
            {
                WorldManager.SpriteBatch.DrawString(font, options[currentOption], rightPosition, Color.White);
            }
        }

        public void DrawHighlighted()
        {
            WorldManager.SpriteBatch.DrawString(font, title, leftPosition, Color.GreenYellow);

            if (options != null)
            {
                WorldManager.SpriteBatch.DrawString(font, options[currentOption], rightPosition, Color.GreenYellow);
            }
        }
    }
}
