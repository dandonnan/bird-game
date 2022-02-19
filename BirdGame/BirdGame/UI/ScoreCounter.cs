namespace BirdGame.UI
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    /// <summary>
    /// The score counter.
    /// </summary>
    internal class ScoreCounter
    {
        /// <summary>
        /// The maximum score.
        /// </summary>
        private const int MaxScore = 999999;

        /// <summary>
        /// The singleton for the score counter, so there can only be one.
        /// </summary>
        private static ScoreCounter scoreCounter;

        /// <summary>
        /// A list of score popups to display.
        /// </summary>
        private readonly List<ScorePopup> scorePopups;

        /// <summary>
        /// The font of the score.
        /// </summary>
        private readonly SpriteFont font;

        /// <summary>
        /// The position of the score.
        /// </summary>
        private Vector2 scorePosition;

        /// <summary>
        /// The target (current) score.
        /// </summary>
        private int targetScore;

        /// <summary>
        /// The display score, to count up to the target.
        /// </summary>
        private int displayScore;

        /// <summary>
        /// The private constructor for the score counter, so it can only
        /// be created from the Initialise method.
        /// </summary>
        private ScoreCounter()
        {
            scorePopups = new List<ScorePopup>();

            // Load the font
            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            // Set the position
            scorePosition = new Vector2(290, 10);

            targetScore = 0;
            displayScore = 0;

            scoreCounter = this;
        }

        /// <summary>
        /// A publically accessible property to get the current score.
        /// </summary>
        public static int CurrentScore => scoreCounter.targetScore;

        /// <summary>
        /// Initialise the score counter.
        /// </summary>
        /// <returns>The score counter.</returns>
        public static ScoreCounter Initialise()
        {
            ScoreCounter counter = scoreCounter;

            if (scoreCounter == null)
            {
                counter = new ScoreCounter();
            }

            return counter;
        }

        /// <summary>
        /// Add a score popup based on the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public static void Add(Target target)
        {
            scoreCounter.scorePopups.Add(new ScorePopup(target));
        }

        /// <summary>
        /// Reset the counter.
        /// </summary>
        public void Reset()
        {
            // Remove all popups
            scorePopups.Clear();

            // Set both counters to 0
            targetScore = 0;
            displayScore = 0;
        }

        /// <summary>
        /// Update the score counter.
        /// </summary>
        public void Update()
        {
            // Get the score popup from the hide points event
            object pointsToHide = EventManager.GetEventObject(KnownEvents.HidePoints);

            // Get the number of points to award from the points awarded event
            object pointsAwarded = EventManager.GetEventObject(KnownEvents.PointsAwarded);

            // If the score popup object exists
            if (pointsToHide != null)
            {
                // Remove it from the list
                scorePopups.Remove((ScorePopup)pointsToHide);
            }

            // If there are points to be awarded
            if (pointsAwarded != null)
            {
                // Increase the target score
                targetScore += (int)pointsAwarded;

                // If the target score is greater than the max
                if (targetScore > MaxScore)
                {
                    // Set the target score back to the max
                    targetScore = MaxScore;
                }
            }

            // If the display score is less than the target score
            if (displayScore < targetScore)
            {
                // Increase the display score
                displayScore++;

                // Change the left position to draw the score so it
                // stays on screen when going over certain values
                if (displayScore < 100)
                {
                    scorePosition.X = 290;
                }
                else if (displayScore < 1000)
                {
                    scorePosition.X = 280;
                }
                else if (displayScore < 10000)
                {
                    scorePosition.X = 270;
                }
                else if (displayScore < 100000)
                {
                    scorePosition.X = 260;
                }
                else
                {
                    scorePosition.X = 250;
                }
            }

            // For each of the score popups
            foreach (ScorePopup score in scorePopups)
            {
                // Update the popup
                score.Update();
            }
        }

        /// <summary>
        /// Draw the counter.
        /// </summary>
        public void Draw()
        {
            // Draw the score in the top right
            WorldManager.SpriteBatch.DrawString(font, displayScore.ToString(), scorePosition, Color.White);

            // For each of the score popups
            foreach (ScorePopup score in scorePopups)
            {
                // Draw the popup
                score.Draw();
            }
        }
    }
}
