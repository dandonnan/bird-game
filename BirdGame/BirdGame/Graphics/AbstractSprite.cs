namespace BirdGame.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// An abstract sprite that contains properties common
    /// between different sprites.
    /// </summary>
    internal abstract class AbstractSprite
    {
        /// <summary>
        /// The texture the sprite uses.
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// The position to draw the sprite.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The rotation of the sprite.
        /// </summary>
        protected float rotation;

        /// <summary>
        /// The scale of the sprite.
        /// </summary>
        protected Vector2 scale;

        /// <summary>
        /// The origin (point of rotation) of the sprite.
        /// </summary>
        protected Vector2 origin;

        /// <summary>
        /// The depth of the sprite. Sprites with higher depth will
        /// draw above other sprites.
        /// </summary>
        protected int depth;

        /// <summary>
        /// The constructor for a sprite.
        /// </summary>
        public AbstractSprite()
        {
            rotation = 0;
            depth = 0;
            scale = Vector2.One;
            origin = Vector2.Zero;
        }

        /// <summary>
        /// Set the position of the sprite.
        /// </summary>
        /// <param name="position">The position.</param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Move the sprite by the given amount.
        /// </summary>
        /// <param name="position">The amount to move.</param>
        public void Move(Vector2 position)
        {
            this.position += position;
        }

        /// <summary>
        /// Move the sprite by the given amounts.
        /// </summary>
        /// <param name="x">The amount to move in the x-axis.</param>
        /// <param name="y">The amount to move in the y-axis.</param>
        public void Move(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        /// <summary>
        /// Set the rotation of the sprite.
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        /// <summary>
        /// Set the scale of the sprite.
        /// </summary>
        /// <param name="scale">The scale to set in both axis.</param>
        public void SetScale(float scale)
        {
            this.scale.X = scale;
            this.scale.Y = scale;
        }

        /// <summary>
        /// Set the scale of the sprite.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }

        /// <summary>
        /// Set the origin of the sprite.
        /// </summary>
        /// <param name="origin">The origin.</param>
        public void SetOrigin(Vector2 origin)
        {
            this.origin = origin;
        }

        /// <summary>
        /// Get the width of the sprite. The classes that inherit
        /// from this have to implement this method.
        /// </summary>
        /// <returns>The width.</returns>
        public abstract int GetWidth();

        /// <summary>
        /// Get the height of the sprite. The classes that inherit
        /// from this have to implement this method.
        /// </summary>
        /// <returns>The height.</returns>
        public abstract int GetHeight();

        /// <summary>
        /// Update the sprite. The classes that inherit from this
        /// have to implement this method.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw the sprite. The classes that inherit from this have
        /// to implement this method.
        /// </summary>
        public abstract void Draw();
    }
}
