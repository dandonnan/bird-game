namespace BirdGame.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal abstract class AbstractSprite
    {
        protected Texture2D texture;

        protected Vector2 position;

        protected float rotation;

        protected Vector2 scale;

        protected Vector2 origin;

        public AbstractSprite()
        {
            rotation = 0;
            scale = Vector2.One;
            origin = Vector2.Zero;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Move(Vector2 position)
        {
            this.position += position;
        }

        public void Move(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public void SetScale(float scale)
        {
            this.scale.X = scale;
            this.scale.Y = scale;
        }

        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }

        public void SetOrigin(Vector2 origin)
        {
            this.origin = origin;
        }

        public abstract int GetWidth();

        public abstract int GetHeight();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();
    }
}
