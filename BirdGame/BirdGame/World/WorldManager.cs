namespace BirdGame.World
{
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

        private readonly SpriteBatch spriteBatch;

        private readonly EventManager eventManager;

        private readonly InputManager inputManager;

        private readonly GameWorld gameWorld;

        private WorldManager(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            this.contentManager = contentManager;
            this.spriteBatch = spriteBatch;

            worldManager = this;

            eventManager = EventManager.Initialise();
            inputManager = InputManager.Initialise();

            SpriteLibrary.Initialise();
            StringLibrary.Initialise();

            gameWorld = new GameWorld();
        }

        public static WorldManager Initialise(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            return new WorldManager(contentManager, spriteBatch);
        }

        public static SpriteBatch SpriteBatch => worldManager.spriteBatch;

        public static ContentManager ContentManager => worldManager.contentManager;

        public void Update(GameTime gameTime)
        {
            inputManager.Update();

            eventManager.Update();

            gameWorld.Update(gameTime);
        }

        public void Draw()
        {
            spriteBatch.Begin();

            gameWorld.Draw();

            spriteBatch.End();
        }
    }
}
