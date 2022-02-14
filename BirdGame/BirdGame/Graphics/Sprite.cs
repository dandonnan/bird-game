﻿namespace BirdGame.Graphics
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class Sprite : AbstractSprite
    {
        private readonly Frame frame;

        public Sprite(Texture2D texture, Frame frame)
            : base()
        {
            this.texture = texture;
            this.frame = frame;
        }

        public override int GetWidth()
        {
            return frame.Rectangle.Width;
        }

        public override int GetHeight()
        {
            return frame.Rectangle.Height;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            WorldManager.SpriteBatch.Draw(texture, position, frame.Rectangle,
                Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}