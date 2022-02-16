namespace BirdGame.Characters
{
    using Microsoft.Xna.Framework;

    internal abstract class AbstractCharacter
    {
        public Vector2 Position => position;

        public Vector2 Origin => origin;

        protected Vector2 position;

        protected Vector2 origin;

        public abstract int GetWidth();

        public abstract int GetHeight();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();
    }
}
