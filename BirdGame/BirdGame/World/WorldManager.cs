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

    /// <summary>
    /// A manager for the world.
    /// </summary>
    internal class WorldManager
    {
        /// <summary>
        /// A singleton instance for the world manager.
        /// </summary>
        private static WorldManager worldManager;

        /// <summary>
        /// The content manager to load in art and audio.
        /// </summary>
        private readonly ContentManager contentManager;

        /// <summary>
        /// The graphics device manager that controls how the game draws.
        /// </summary>
        private readonly GraphicsDeviceManager graphicsDeviceManager;

        /// <summary>
        /// The sprite batch that determines how sprites are drawn.
        /// </summary>
        private readonly SpriteBatch spriteBatch;

        /// <summary>
        /// The event manager.
        /// </summary>
        private readonly EventManager eventManager;

        /// <summary>
        /// The input manager.
        /// </summary>
        private readonly InputManager inputManager;

        /// <summary>
        /// The game world.
        /// </summary>
        private readonly GameWorld gameWorld;

        /// <summary>
        /// The constructor for the world manager. This is private so it can only
        /// be created through the Initialise method.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        private WorldManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch spriteBatch)
        {
            this.contentManager = contentManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.spriteBatch = spriteBatch;

            worldManager = this;

            // Initialise everything
            eventManager = EventManager.Initialise();
            inputManager = InputManager.Initialise();

            SpriteLibrary.Initialise();
            StringLibrary.Initialise();
            AudioLibrary.Initialise();
            AudioManager.Initialise();
            SaveManager.Initialise();

            gameWorld = new GameWorld();
        }

        /// <summary>
        /// Initialise the world manager.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <returns>The world manager.</returns>
        public static WorldManager Initialise(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch spriteBatch)
        {
            return new WorldManager(contentManager, graphicsDeviceManager, spriteBatch);
        }

        /// <summary>
        /// A publically accessible sprite batch that other classes can use.
        /// </summary>
        public static SpriteBatch SpriteBatch => worldManager.spriteBatch;

        /// <summary>
        /// A publically accessible graphics device manager that other classes can use.
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager => worldManager.graphicsDeviceManager;

        /// <summary>
        /// A publically accessible content manager that other classes can use.
        /// </summary>
        public static ContentManager ContentManager => worldManager.contentManager;

        /// <summary>
        /// A publically accessible game world that other classes can use.
        /// </summary>
        public static GameWorld GameWorld => worldManager.gameWorld;

        /// <summary>
        /// Update the world manager.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public void Update(GameTime gameTime)
        {
            // Update the managers
            inputManager.Update();

            eventManager.Update();

            // Update the world
            gameWorld.Update(gameTime);
        }

        /// <summary>
        /// Draw the world.
        /// </summary>
        public void Draw()
        {
            gameWorld.Draw();
        }
    }
}
