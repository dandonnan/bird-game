namespace BirdGame.UI
{
    using BirdGame.Graphics;
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

        private readonly Sprite background;

        public ScoreScreen(int currentScore, int highScore)
        {
            background = SpriteLibrary.GetSprite("UiBackground");
            background.SetPosition(new Vector2(-32, -32));
            background.SetScale(25);

            currentScoreText = currentScore.ToString();

            int screenWidth = MainGame.DefaultWidth;

            highScoreFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\NewHighScore");
            personalBestFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            if (currentScore > highScore)
            {
                newHighScoreText = StringLibrary.GetString("NewScore");
                newHighScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(newHighScoreText).X) / 2, 50);
            }

            if (highScore > 0)
            {
                personalBestText = StringLibrary.GetString("PersonalBest");

                if (personalBestText != null)
                {
                    personalBestText = string.Format(personalBestText, highScore);
                    personalBestPosition = new Vector2((screenWidth - personalBestFont.MeasureString(personalBestText).X) / 2, 150);
                }
            }

            currentScorePosition = new Vector2((screenWidth - highScoreFont.MeasureString(currentScoreText).X) / 2, 90);
        }

        public void Draw()
        {
            background.Draw();

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
