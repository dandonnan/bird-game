namespace BirdGame
{
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MainGame : Game
    {
        private static MainGame mainGame;

        public const int DefaultWidth = 320;

        public const int DefaultHeight = 240;

        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;

        private WorldManager worldManager;

        public MainGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            mainGame = this;
        }

        public static void QuitGame()
        {
            mainGame.Exit();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            worldManager = WorldManager.Initialise(Content, graphicsDeviceManager, spriteBatch);

            Window.Title = StringLibrary.GetString("GameTitle");

            graphicsDeviceManager.IsFullScreen = SaveManager.SaveData.Fullscreen;

            if (graphicsDeviceManager.IsFullScreen)
            {
                graphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                graphicsDeviceManager.PreferredBackBufferWidth = SaveManager.SaveData.ResolutionWidth;
                graphicsDeviceManager.PreferredBackBufferHeight = SaveManager.SaveData.ResolutionHeight;
            }

            graphicsDeviceManager.ApplyChanges();

            EventManager.FireEvent(KnownEvents.ResolutionChanged);
        }

        protected override void Update(GameTime gameTime)
        {
            worldManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            worldManager.Draw();

            base.Draw(gameTime);
        }
    }
}
