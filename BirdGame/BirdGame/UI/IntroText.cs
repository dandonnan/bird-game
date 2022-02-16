namespace BirdGame.UI
{
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Input;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class IntroText
    {
        private const float textMoveSpeed = 1.5f;

        private const float titleFinalPositionY = -50;

        private const float copyrightFinalPositionY = 300;

        private readonly SpriteFont titleFont;

        private readonly SpriteFont copyrightFont;

        private string gameTitle;

        private string copyright;

        private Vector2 titlePosition;

        private Vector2 copyrightPosition;

        private bool transitionOut = false;

        private bool finished = false;

        public IntroText()
        {
            titleFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\GameTitle");
            copyrightFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\PersonalBest");

            Reset();
        }

        public void Reset()
        {
            gameTitle = StringLibrary.GetString("GameTitle");
            copyright = StringLibrary.GetString("Copyright");

            int screenWidth = MainGame.DefaultWidth;

            titlePosition = new Vector2((screenWidth - titleFont.MeasureString(gameTitle).X) / 2, 10);
            copyrightPosition = new Vector2((screenWidth - copyrightFont.MeasureString(copyright).X) / 2, 200);
        }

        public void BeginTransitionOut()
        {
            transitionOut = true;
        }

        public void Update()
        {
            if (transitionOut && finished == false)
            {
                TransitionOut();
            }
            else if (finished == false)
            {
                if (InputManager.IsBindingPressed(DefaultBindings.Dive)
                    || InputManager.IsBindingPressed(DefaultBindings.Poop)
                    || InputManager.IsBindingPressed(DefaultBindings.Up)
                    || InputManager.IsBindingPressed(DefaultBindings.Down)
                    || InputManager.IsBindingPressed(DefaultBindings.Left)
                    || InputManager.IsBindingPressed(DefaultBindings.Right))
                {
                    transitionOut = true;
                    EventManager.FireEvent(KnownEvents.SpawnBird);
                }
            }
        }

        public void Draw()
        {
            if (finished == false)
            {
                WorldManager.SpriteBatch.DrawString(titleFont, gameTitle, titlePosition, Color.White);
                WorldManager.SpriteBatch.DrawString(copyrightFont, copyright, copyrightPosition, Color.CornflowerBlue);
            }
        }

        private void TransitionOut()
        {
            bool textAtEnd = false;

            if (titlePosition.Y > titleFinalPositionY)
            {
                titlePosition.Y -= textMoveSpeed;
            }
            else
            {
                textAtEnd = true;
            }

            if (copyrightPosition.Y < copyrightFinalPositionY)
            {
                copyrightPosition.Y += textMoveSpeed;
                textAtEnd = false;
            }

            if (textAtEnd)
            {
                finished = true;
                EventManager.FireEvent(KnownEvents.IntroFinished);
            }
        }
    }
}
