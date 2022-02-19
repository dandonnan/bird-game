namespace BirdGame.UI
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A popup showing the score when a poop hits or the bird dives onto a target.
    /// </summary>
    internal class ScorePopup
    {
        /// <summary>
        /// The font.
        /// </summary>
        private readonly SpriteFont font;

        /// <summary>
        /// The target y position.
        /// </summary>
        private readonly float targetY;

        /// <summary>
        /// The text to display.
        /// </summary>
        private string scoreText;

        /// <summary>
        /// The points to award.
        /// </summary>
        private string pointsAwarded;

        /// <summary>
        /// The position of the text.
        /// </summary>
        private Vector2 textPosition;

        /// <summary>
        /// The position of the score.
        /// </summary>
        private Vector2 pointsPosition;

        /// <summary>
        /// A constructor for a score popup.
        /// </summary>
        /// <param name="target">The target that was hit.</param>
        public ScorePopup(Target target)
        {
            // Set the positions of the text
            textPosition = new Vector2(MainGame.DefaultWidth / 4, MainGame.DefaultHeight / 4);
            pointsPosition = new Vector2(textPosition.X + 50, textPosition.Y - 30);

            // Set the target y position to be 100 above the current position
            targetY = textPosition.Y - 100;

            // Load the font
            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\TargetHit");

            // Get the points strings based on the target
            GetPoints(target);
        }

        /// <summary>
        /// Update the popup.
        /// </summary>
        public void Update()
        {
            // If the text position is above the target
            if (textPosition.Y < targetY)
            {
                // Fire an event to hide the popup
                EventManager.FireEvent(KnownEvents.HidePoints, this);
            }
            // If the text position is below the target
            else
            {
                // Move the text up
                textPosition.Y--;
                pointsPosition.Y--;
            }
        }

        /// <summary>
        /// Draw the text.
        /// </summary>
        public void Draw()
        {
            WorldManager.SpriteBatch.DrawString(font, scoreText, textPosition, Color.CornflowerBlue);
            WorldManager.SpriteBatch.DrawString(font, pointsAwarded, pointsPosition, Color.GreenYellow);
        }

        /// <summary>
        /// Get points from the target.
        /// </summary>
        /// <param name="target">The target.</param>
        private void GetPoints(Target target)
        {
            int points = 0;

            switch (target)
            {
                case Target.PoopHead:
                    scoreText = StringLibrary.GetString("PoopHead");
                    points = 50;
                    break;

                case Target.PoopJacket:
                    scoreText = StringLibrary.GetString("PoopJacket");
                    points = 20;
                    break;

                case Target.PoopCoffee:
                    scoreText = StringLibrary.GetString("PoopCoffee");
                    points = 100;
                    break;

                case Target.PoopPig:
                    scoreText = StringLibrary.GetString("PoopPig");
                    points = 30;
                    break;

                case Target.PoopCar:
                    scoreText = StringLibrary.GetString("PoopCar");
                    points = 10;
                    break;

                case Target.PoopIceCream:
                    scoreText = StringLibrary.GetString("PoopIceCream");
                    points = 150;
                    break;

                case Target.PoopChips:
                    scoreText = StringLibrary.GetString("PoopChips");
                    points = 75;
                    break;

                case Target.DiveCoffee:
                    scoreText = StringLibrary.GetString("DiveCoffee");
                    points = 100;
                    break;

                case Target.DivePig:
                    scoreText = StringLibrary.GetString("DivePig");
                    points = 50;
                    break;

                case Target.DiveIceCream:
                    scoreText = StringLibrary.GetString("DiveIceCream");
                    points = 100;
                    break;

                case Target.DiveChips:
                    scoreText = StringLibrary.GetString("DiveChips");
                    points = 100;
                    break;
            }

            pointsAwarded = points.ToString();

            // Fire an event so that the score counter increases the number of points
            EventManager.FireEvent(KnownEvents.PointsAwarded, points);
        }
    }
}
