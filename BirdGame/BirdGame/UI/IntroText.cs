namespace BirdGame.UI
{
    using BirdGame.Events;
    using BirdGame.Input;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The intro text screen.
    /// </summary>
    internal class IntroText
    {
        /// <summary>
        /// The speed to move the text by.
        /// </summary>
        private const float textMoveSpeed = 1.5f;

        /// <summary>
        /// The final position of the title.
        /// </summary>
        private const float titleFinalPositionY = -50;

        /// <summary>
        /// The font to draw the title with.
        /// </summary>
        private readonly SpriteFont titleFont;

        /// <summary>
        /// The game's title.
        /// </summary>
        private string gameTitle;

        /// <summary>
        /// The position of the title text.
        /// </summary>
        private Vector2 titlePosition;

        /// <summary>
        /// Whether the title is transitioning out.
        /// </summary>
        private bool transitionOut = false;

        /// <summary>
        /// Whether the title has reached the final position.
        /// </summary>
        private bool finished = false;

        /// <summary>
        /// A constructor for the intro text screen.
        /// </summary>
        public IntroText()
        {
            // Load the font
            titleFont = WorldManager.ContentManager.Load<SpriteFont>("Fonts\\GameTitle");

            Reset();
        }

        /// <summary>
        /// Reset the screen.
        /// </summary>
        public void Reset()
        {
            // Get the title from the string library
            gameTitle = StringLibrary.GetString("GameTitle");

            // Get the width of the screen
            int screenWidth = MainGame.DefaultWidth;

            // Set the initial position of the title text to be aligned to the center
            titlePosition = new Vector2((screenWidth - titleFont.MeasureString(gameTitle).X) / 2, 10);
        }

        /// <summary>
        /// Update the screen.
        /// </summary>
        public void Update()
        {
            // If transitioning out and not yet finished
            if (transitionOut && finished == false)
            {
                // Transition out
                TransitionOut();
            }
            // If the text has not finished moving and is not transitioning
            else if (finished == false)
            {
                // Wait for the player to hit one of the inputs
                if (InputManager.IsBindingPressed(DefaultBindings.Dive)
                    || InputManager.IsBindingPressed(DefaultBindings.Poop)
                    || InputManager.IsBindingPressed(DefaultBindings.Up)
                    || InputManager.IsBindingPressed(DefaultBindings.Down)
                    || InputManager.IsBindingPressed(DefaultBindings.Left)
                    || InputManager.IsBindingPressed(DefaultBindings.Right))
                {
                    // Start the transition out
                    transitionOut = true;

                    // Fire an event to spawn the bird
                    EventManager.FireEvent(KnownEvents.SpawnBird);
                }
            }
        }

        /// <summary>
        /// Draw the text.
        /// </summary>
        public void Draw()
        {
            // If the text has not finished moving
            if (finished == false)
            {
                // Draw the text
                WorldManager.SpriteBatch.DrawString(titleFont, gameTitle, titlePosition, Color.White);
            }
        }

        /// <summary>
        /// Transition the text off the screen.
        /// </summary>
        private void TransitionOut()
        {
            // If the title is below the final position
            if (titlePosition.Y > titleFinalPositionY)
            {
                // Move the title up
                titlePosition.Y -= textMoveSpeed;
            }
            // If the title is above or at the final position
            else
            {
                // The screen has finished
                finished = true;

                // Trigger the event to say the intro has finished
                EventManager.FireEvent(KnownEvents.IntroFinished);
            }
        }
    }
}
