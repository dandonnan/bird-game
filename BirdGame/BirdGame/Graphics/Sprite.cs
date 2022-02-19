namespace BirdGame.Graphics
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A non-animated sprite.
    /// </summary>
    internal class Sprite : AbstractSprite
    {
        /// <summary>
        /// The frame of the sprite, indicating where on the texture it is.
        /// </summary>
        private readonly Frame frame;

        /// <summary>
        /// Create a sprite.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="frame">The frame where the sprite is on the texture.</param>
        public Sprite(Texture2D texture, Frame frame)
            : base()
        {
            this.texture = texture;
            this.frame = frame;
        }

        /// <summary>
        /// Create a sprite from another sprite.
        /// </summary>
        /// <param name="sprite">The sprite to copy from.</param>
        public Sprite(Sprite sprite)
        {
            this.texture = sprite.texture;
            this.frame = new Frame(sprite.frame.Rectangle.X, sprite.frame.Rectangle.Y,
                    sprite.frame.Rectangle.Width, sprite.frame.Rectangle.Height);
            this.rotation = sprite.rotation;
            this.scale = sprite.scale;
            this.origin = sprite.origin;
        }

        /// <summary>
        /// Get the width of the sprite.
        /// </summary>
        /// <returns>The width.</returns>
        public override int GetWidth()
        {
            return frame.Rectangle.Width;
        }

        /// <summary>
        /// Get the height of the sprite.
        /// </summary>
        /// <returns>The height.</returns>
        public override int GetHeight()
        {
            return frame.Rectangle.Height;
        }

        /// <summary>
        /// The update method. As the sprite is not animated,
        /// there is nothing to update.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw the sprite.
        /// </summary>
        public override void Draw()
        {
            WorldManager.SpriteBatch.Draw(texture, position, frame.Rectangle,
                Color.White, rotation, origin, scale, SpriteEffects.None, depth);
        }
    }
}
