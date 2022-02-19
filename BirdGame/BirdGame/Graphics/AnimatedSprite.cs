namespace BirdGame.Graphics
{
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An animated sprite.
    /// </summary>
    internal class AnimatedSprite : AbstractSprite
    {
        /// <summary>
        /// A list of frames to use in the animation.
        /// </summary>
        private readonly List<Frame> frames;

        /// <summary>
        /// Whether the sprite loops.
        /// </summary>
        private readonly bool looping;

        /// <summary>
        /// The speed to update the sprite at. 100 is 0.1 seconds.
        /// </summary>
        private double frameSpeed = 100;

        /// <summary>
        /// The index of the current frame.
        /// </summary>
        private int currentFrame;

        /// <summary>
        /// The time since the last frame was drawn.
        /// </summary>
        private double timeSinceLastFrame;

        /// <summary>
        /// Create a new animated sprite which loops.
        /// </summary>
        /// <param name="texture">The texture to draw from.</param>
        /// <param name="frames">A list of frames for animation.</param>
        public AnimatedSprite(Texture2D texture, List<Frame> frames)
            : this(texture, frames, true)
        {
            // This does not do anything, since it calls the next constructor
            // which sets everything up
        }

        /// <summary>
        /// Create a new animated sprite.
        /// </summary>
        /// <param name="texture">The texture to draw from.</param>
        /// <param name="frames">A list of frames for animation.</param>
        /// <param name="looping">Whether the sprite loops.</param>
        public AnimatedSprite(Texture2D texture, List<Frame> frames, bool looping)
            : base()
        {
            this.texture = texture;
            this.frames = frames;
            currentFrame = 0;
            this.looping = looping;
        }

        /// <summary>
        /// Create an animated sprite from another sprite.
        /// </summary>
        /// <param name="sprite">The sprite to copy from.</param>
        public AnimatedSprite(AnimatedSprite sprite)
        {
            texture = sprite.texture;
            frames = new List<Frame>(sprite.frames);
            currentFrame = 0;
            looping = sprite.looping;
            rotation = sprite.rotation;
            scale = sprite.scale;
            origin = sprite.origin;
        }

        /// <summary>
        /// A publically accessible property to get the current frame.
        /// </summary>
        public int CurrentFrame => currentFrame;

        /// <summary>
        /// A publically accessible property to get the last frame in the animation.
        /// </summary>
        public int LastFrame => frames.Count - 1;

        /// <summary>
        /// Get whether the animation is at the end.
        /// </summary>
        /// <returns>true if the animation is at the end, false if not or the animation loops.</returns>
        public bool IsAnimationAtEnd()
        {
            bool atEnd = false;

            // If the animation does not loop
            if (looping == false)
            {
                // Set at end to be true if the current frame is the same as the last frame
                atEnd = currentFrame == frames.Count - 1;
            }

            return atEnd;
        }

        /// <summary>
        /// Set the speed to update the sprite at.
        /// </summary>
        /// <param name="speed">The sprite.</param>
        public void SetFrameSpeed(float speed)
        {
            frameSpeed = speed;
        }

        /// <summary>
        /// Reset the sprite to the first frame.
        /// </summary>
        public void Reset()
        {
            currentFrame = 0;
        }

        /// <summary>
        /// Get the width of the sprite.
        /// </summary>
        /// <returns>The width.</returns>
        public override int GetWidth()
        {
            // Use the width from the first frame in the animation
            return frames.First().Rectangle.Width;
        }

        /// <summary>
        /// Get the height of the sprite.
        /// </summary>
        /// <returns>The height.</returns>
        public override int GetHeight()
        {
            // Use the height from the first frame in the animation
            return frames.First().Rectangle.Height;
        }

        /// <summary>
        /// Update the sprite.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
            // If the animation is not at the end
            if (IsAnimationAtEnd() == false)
            {
                // Increase the time since the last frame based on the game time
                timeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

                // If the time since the last frame has gone over the frame speed
                if (timeSinceLastFrame >= frameSpeed)
                {
                    // Reset the time
                    timeSinceLastFrame = 0;

                    // Increase the current frame
                    currentFrame++;

                    // If the current frame is at the end of the list
                    if (currentFrame >= frames.Count)
                    {
                        // If the animation is not looping
                        if (looping == false)
                        {
                            // Set the current frame to be the last frame
                            currentFrame = frames.Count - 1;
                        }
                        else
                        {
                            // If the animation is looping, set the current frame to 0 to start again
                            currentFrame = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw the sprite.
        /// </summary>
        public override void Draw()
        {
            WorldManager.SpriteBatch.Draw(texture, position, frames[currentFrame].Rectangle,
                Color.White, rotation, origin, scale, SpriteEffects.None, depth);
        }
    }
}
