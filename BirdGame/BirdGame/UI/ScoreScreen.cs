namespace BirdGame.UI
{
    using BirdGame.Graphics;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The score screen at the end of the game.
    /// </summary>
    internal class ScoreScreen
    {
        /// <summary>
        /// The font for the high score.
        /// </summary>
        private readonly SpriteFont highScoreFont;

        /// <summary>
        /// The font for the personal best.
        /// </summary>
        private readonly SpriteFont personalBestFont;

        /// <summary>
        /// The text to display the current score.
        /// </summary>
        private readonly string currentScoreText;

        /// <summary>
        /// The text for new high score.
        /// </summary>
        private readonly string newHighScoreText;

        /// <summary>
        /// The text for the personal best text.
        /// </summary>
        private readonly string personalBestText;

        /// <summary>
        /// The position of the current score.
        /// </summary>
        private readonly Vector2 currentScorePosition;

        /// <summary>
        /// The position of the new high score text.
        /// </summary>
        private readonly Vector2 newHighScorePosition;

        /// <summary>
        /// The position of the personal best text.
        /// </summary>
        private readonly Vector2 personalBestPosition;

        /// <summary>
        /// The sprite to show in the background.
        /// </summary>
        private readonly Sprite background;

        /// <summary>
        /// A constructor for the score screen.
        /// </summary>
        /// <param name="currentScore">The current score.</param>
        /// <param name="highScore">The player's high score.</param>
        public ScoreScreen(int currentScore, int highScore)
        {
            // Get the background from the library
            background = SpriteLibrary.GetSprite("UiBackground");

            // Set it to be off screen
            background.SetPosition(new Vector2(-32, -32));

            // Set the scale to make it bigger than the screen so it draws on it all
            background.SetScale(25);

            // Convert the score to a string
            currentScoreText = currentScore.ToString();

            // Get the width of the screen
            int screenWidth = MainGame.DefaultWidth;

            // Load the fonts
            highScoreFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\NewHighScore");
            personalBestFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            // If the current score is greater than the high score
            if (currentScore > highScore)
            {
                // Set the new high score text and the position of the new high score text
                newHighScoreText = StringLibrary.GetString("NewScore");
                newHighScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(newHighScoreText).X) / 2, 50);
            }

            // If the high score is greater than 0
            if (highScore > 0)
            {
                // Get the personal best text
                personalBestText = StringLibrary.GetString("PersonalBest");

                // If there is a string for the personal best
                if (personalBestText != null)
                {
                    // Set the score in the personal best text, by replacing the {0} in the string with the high score
                    personalBestText = string.Format(personalBestText, highScore);

                    // Set the position of the personal best text
                    personalBestPosition = new Vector2((screenWidth - personalBestFont.MeasureString(personalBestText).X) / 2, 150);
                }
            }

            // Set the position of the current score
            currentScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(currentScoreText).X) / 2, 90);
        }

        /// <summary>
        /// Draw the score screen.
        /// </summary>
        public void Draw()
        {
            // Draw the background
            background.Draw();

            // If the new high score text is set
            if (string.IsNullOrEmpty(newHighScoreText) == false)
            {
                // Draw the new high score text
                WorldManager.SpriteBatch.DrawString(highScoreFont, newHighScoreText, newHighScorePosition, Color.White);
            }

            // Draw the current score
            WorldManager.SpriteBatch.DrawString(highScoreFont, currentScoreText, currentScorePosition, Color.White);

            // If the personal best text is set
            if (string.IsNullOrEmpty(personalBestText) == false)
            {
                // Draw the personal best
                WorldManager.SpriteBatch.DrawString(personalBestFont, personalBestText, personalBestPosition, Color.White);
            }
        }
    }
}
