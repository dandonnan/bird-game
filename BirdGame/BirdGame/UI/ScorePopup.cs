namespace BirdGame.UI
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class ScorePopup
    {
        private readonly SpriteFont font;

        private readonly float targetY;

        private string scoreText;

        private string pointsAwarded;

        private Vector2 textPosition;

        private Vector2 pointsPosition;

        public ScorePopup(Target target)
        {
            textPosition = new Vector2(MainGame.DefaultWidth / 4, MainGame.DefaultHeight / 4);
            pointsPosition = new Vector2(textPosition.X + 50, textPosition.Y - 30);

            targetY = textPosition.Y - 100;

            font = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\TargetHit");

            GetPoints(target);
        }

        public void Update()
        {
            if (textPosition.Y < targetY)
            {
                EventManager.FireEvent(KnownEvents.HidePoints, this);
            }
            else
            {
                textPosition.Y--;
                pointsPosition.Y--;
            }
        }

        public void Draw()
        {
            WorldManager.SpriteBatch.DrawString(font, scoreText, textPosition, Color.CornflowerBlue);
            WorldManager.SpriteBatch.DrawString(font, pointsAwarded, pointsPosition, Color.GreenYellow);
        }

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
            EventManager.FireEvent(KnownEvents.PointsAwarded, points);
        }
    }
}
