namespace BirdGame.UI
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    internal class ScoreCounter
    {
        private const int MaxScore = 999999;

        private static ScoreCounter scoreCounter;

        private readonly List<ScorePopup> scorePopups;

        private readonly SpriteFont font;

        private readonly Vector2 scorePosition;

        private int targetScore;

        private int displayScore;

        private ScoreCounter()
        {
            scorePopups = new List<ScorePopup>();

            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            scorePosition = new Vector2(290, 10);

            targetScore = 0;
            displayScore = 0;

            scoreCounter = this;
        }

        public static ScoreCounter Initialise()
        {
            ScoreCounter counter = scoreCounter;

            if (scoreCounter == null)
            {
                counter = new ScoreCounter();
            }

            return counter;
        }

        public static void Add(Vector2 position, Target target)
        {
            scoreCounter.scorePopups.Add(new ScorePopup(position, target));
        }

        public void Update()
        {
            object pointsToHide = EventManager.GetEventObject(KnownEvents.HidePoints);
            object pointsAwarded = EventManager.GetEventObject(KnownEvents.PointsAwarded);

            if (pointsToHide != null)
            {
                scorePopups.Remove((ScorePopup)pointsToHide);
            }

            if (pointsAwarded != null)
            {
                targetScore += (int)pointsAwarded;

                if (targetScore > MaxScore)
                {
                    targetScore = MaxScore;
                }
            }

            if (displayScore < targetScore)
            {
                displayScore++;
            }

            foreach (ScorePopup score in scorePopups)
            {
                score.Update();
            }
        }

        public void Draw()
        {
            WorldManager.SpriteBatch.DrawString(font, displayScore.ToString(), scorePosition, Color.White);

            foreach (ScorePopup score in scorePopups)
            {
                score.Draw();
            }
        }
    }
}
