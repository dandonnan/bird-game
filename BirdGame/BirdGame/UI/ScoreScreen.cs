namespace BirdGame.UI
{
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class ScoreScreen
    {
        private readonly SpriteFont highScoreFont;

        private readonly SpriteFont personalBestFont;

        private readonly string currentScoreText;

        private readonly string newHighScoreText;

        private readonly string personalBestText;

        private readonly Vector2 currentScorePosition;

        private readonly Vector2 newHighScorePosition;

        private readonly Vector2 personalBestPosition;

        public ScoreScreen(int currentScore, int highScore)
        {
            currentScoreText = currentScore.ToString();

            if (currentScore > highScore)
            {
                newHighScoreText = StringLibrary.GetString("NewScore");
            }

            if (highScore > 0)
            {
                personalBestText = StringLibrary.GetString("PersonalBest");

                if (personalBestText != null)
                {
                    personalBestText = string.Format(personalBestText, highScore);
                }
            }

            highScoreFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\NewHighScore");
            personalBestFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            int screenWidth = MainGame.DefaultWidth;

            currentScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(currentScoreText).X) / 2, 90);
            newHighScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(newHighScoreText).X) / 2, 50);
            personalBestPosition = new Vector2((screenWidth - personalBestFont.MeasureString(personalBestText).X) / 2, 150);
        }

        public void Draw()
        {
            if (string.IsNullOrEmpty(newHighScoreText) == false)
            {
                WorldManager.SpriteBatch.DrawString(highScoreFont, newHighScoreText, newHighScorePosition, Color.White);
            }

            WorldManager.SpriteBatch.DrawString(highScoreFont, currentScoreText, currentScorePosition, Color.White);

            if (string.IsNullOrEmpty(personalBestText) == false)
            {
                WorldManager.SpriteBatch.DrawString(personalBestFont, personalBestText, personalBestPosition, Color.White);
            }
        }
    }
}
