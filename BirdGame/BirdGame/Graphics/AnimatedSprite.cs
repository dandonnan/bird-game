namespace BirdGame.Graphics
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class AnimatedSprite : AbstractSprite
    {
        private readonly List<Frame> frames;

        private int currentFrame;

        private bool looping;

        private double frameSpeed = 100;

        private double timeSinceLastFrame;

        public AnimatedSprite(Texture2D texture, List<Frame> frames)
            : this(texture, frames, true)
        {
        }

        public AnimatedSprite(Texture2D texture, List<Frame> frames, bool looping)
            : base()
        {
            this.texture = texture;
            this.frames = frames;
            currentFrame = 0;
            this.looping = looping;
        }

        public bool IsAnimationAtEnd()
        {
            bool atEnd = false;

            if (looping == false)
            {
                atEnd = currentFrame == frames.Count - 1;
            }

            return atEnd;
        }

        public void Reset()
        {
            currentFrame = 0;
        }

        public override int GetWidth()
        {
            return frames.First().Rectangle.Width;
        }

        public override int GetHeight()
        {
            return frames.First().Rectangle.Height;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAnimationAtEnd() == false)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastFrame >= frameSpeed)
                {
                    timeSinceLastFrame = 0;
                    currentFrame++;

                    if (currentFrame >= frames.Count - 1)
                    {
                        if (looping == false)
                        {
                            currentFrame = frames.Count - 1;
                        }
                        else
                        {
                            currentFrame = 0;
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            WorldManager.SpriteBatch.Draw(texture, position, frames[currentFrame].Rectangle,
                Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
