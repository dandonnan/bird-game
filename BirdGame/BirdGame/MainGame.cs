namespace BirdGame
{
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Called Game1 by MonoGame when the project is created, this is the
    /// main entry point.
    /// </summary>
    public class MainGame : Game
    {
        /// <summary>
        /// The singleton for the main game.
        /// </summary>
        private static MainGame mainGame;

        /// <summary>
        /// The default width / base resolution of the game.
        /// </summary>
        public const int DefaultWidth = 320;

        /// <summary>
        /// The default height / base resolution of the game.
        /// </summary>
        public const int DefaultHeight = 240;

        /// <summary>
        /// The graphics device manager.
        /// </summary>
        private GraphicsDeviceManager graphicsDeviceManager;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The world manager.
        /// </summary>
        private WorldManager worldManager;

        /// <summary>
        /// The constructor for the game.
        /// </summary>
        public MainGame()
        {
            // Automatically set by MonoGame
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Set the singleton to this instance
            mainGame = this;
        }

        /// <summary>
        /// Quit the game.
        /// </summary>
        public static void QuitGame()
        {
            mainGame.Exit();
        }

        /// <summary>
        /// Initialise the game.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load the content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialise the world manager
            worldManager = WorldManager.Initialise(Content, graphicsDeviceManager, spriteBatch);

            // Set the title of the game window to the title of the game
            Window.Title = StringLibrary.GetString("GameTitle");

            // Set fullscreen based on the save data
            graphicsDeviceManager.IsFullScreen = SaveManager.SaveData.Fullscreen;

            // If the game is full screen
            if (graphicsDeviceManager.IsFullScreen)
            {
                // Set the width and height to the monitor's screen
                graphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            }
            // If the game is not full screen
            else
            {
                // Set the width and height to the resolution from the settings
                graphicsDeviceManager.PreferredBackBufferWidth = SaveManager.SaveData.ResolutionWidth;
                graphicsDeviceManager.PreferredBackBufferHeight = SaveManager.SaveData.ResolutionHeight;
            }

            // Apply the resolution to the graphics device manager
            graphicsDeviceManager.ApplyChanges();

            // Fire an event saying the resolution has changed so things can update
            EventManager.FireEvent(KnownEvents.ResolutionChanged);
        }

        /// <summary>
        /// Update the game.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        protected override void Update(GameTime gameTime)
        {
            worldManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            worldManager.Draw();

            base.Draw(gameTime);
        }
    }
}
