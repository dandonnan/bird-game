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

        private Vector2 scorePosition;

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

        public static int CurrentScore => scoreCounter.targetScore;

        public static ScoreCounter Initialise()
        {
            ScoreCounter counter = scoreCounter;

            if (scoreCounter == null)
            {
                counter = new ScoreCounter();
            }

            return counter;
        }

        public static void Add(Target target)
        {
            scoreCounter.scorePopups.Add(new ScorePopup(target));
        }

        public void Reset()
        {
            scorePopups.Clear();
            targetScore = 0;
            displayScore = 0;
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
