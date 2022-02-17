namespace BirdGame.Characters
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;

    internal abstract class AbstractCharacter
    {
        public Vector2 Position => position;

        public float Rotation => rotation;

        public Vector2 Origin => origin;

        public float Lifetime { get;  protected set; }

        public float MinLifetime { get; protected set; }

        protected Vector2 position;

        protected float rotation;

        protected Vector2 origin;

        protected SpawnPoint spawnPoint;

        public abstract int GetWidth();

        public abstract int GetHeight();

        public abstract void Kill();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();
    }
}
