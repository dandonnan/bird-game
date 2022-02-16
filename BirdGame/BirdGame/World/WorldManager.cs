namespace BirdGame.World
{
    using BirdGame.Audio;
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.Input;
    using BirdGame.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    internal class WorldManager
    {
        private static WorldManager worldManager;

        private readonly ContentManager contentManager;

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        private readonly SpriteBatch spriteBatch;

        private readonly EventManager eventManager;

        private readonly InputManager inputManager;

        private readonly GameWorld gameWorld;

        private WorldManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch spriteBatch)
        {
            this.contentManager = contentManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.spriteBatch = spriteBatch;

            worldManager = this;

            eventManager = EventManager.Initialise();
            inputManager = InputManager.Initialise();

            SpriteLibrary.Initialise();
            StringLibrary.Initialise();
            AudioLibrary.Initialise();
            AudioManager.Initialise();
            SaveManager.Initialise();

            gameWorld = new GameWorld();
        }

        public static WorldManager Initialise(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch spriteBatch)
        {
            return new WorldManager(contentManager, graphicsDeviceManager, spriteBatch);
        }

        public static SpriteBatch SpriteBatch => worldManager.spriteBatch;

        public static GraphicsDeviceManager GraphicsDeviceManager => worldManager.graphicsDeviceManager;

        public static ContentManager ContentManager => worldManager.contentManager;

        public static GameWorld GameWorld => worldManager.gameWorld;

        public void Update(GameTime gameTime)
        {
            inputManager.Update();

            eventManager.Update();

            gameWorld.Update(gameTime);
        }

        public void Draw()
        {
            gameWorld.Draw();
        }
    }
}
