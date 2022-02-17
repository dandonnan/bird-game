namespace BirdGame.UI
{
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

        private readonly SpriteFont titleFont;

        private string gameTitle;

        private Vector2 titlePosition;

        private bool transitionOut = false;

        private bool finished = false;

        public IntroText()
        {
            titleFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\GameTitle");

            Reset();
        }

        public void Reset()
        {
            gameTitle = StringLibrary.GetString("GameTitle");

            int screenWidth = MainGame.DefaultWidth;

            titlePosition = new Vector2((screenWidth - titleFont.MeasureString(gameTitle).X) / 2, 10);
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
            }
        }

        private void TransitionOut()
        {
            if (titlePosition.Y > titleFinalPositionY)
            {
                titlePosition.Y -= textMoveSpeed;
            }
            else
            {
                finished = true;
                EventManager.FireEvent(KnownEvents.IntroFinished);
            }
        }
    }
}
