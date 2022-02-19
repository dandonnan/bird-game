namespace BirdGame.World
{
    using BirdGame.Audio;
    using BirdGame.Characters;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Poop.
    /// </summary>
    internal class Poop
    {
        /// <summary>
        /// The falling sprite.
        /// </summary>
        private readonly AnimatedSprite fallingSprite;

        /// <summary>
        /// The sprite when the poop has splatted.
        /// </summary>
        private readonly Sprite splatSprite;

        /// <summary>
        /// The position of the poop.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The character the poop is attached to.
        /// </summary>
        private RoamingCharacter attachedCharacter;

        /// <summary>
        /// The offset to the attached character.
        /// </summary>
        private Vector2 attachedOffset;

        /// <summary>
        /// The rotation offset to the attached character.
        /// </summary>
        private float attachedRotation;

        /// <summary>
        /// Whether the poop is falling.
        /// </summary>
        private bool falling;

        /// <summary>
        /// Constructor for the poop.
        /// </summary>
        /// <param name="position">Set the position.</param>
        public Poop(Vector2 position)
        {
            this.position = position;
            falling = true;

            // Get the sprites
            fallingSprite = SpriteLibrary.GetAnimatedSprite("PoopDrop");
            splatSprite = SpriteLibrary.GetSprite("StainPoop");

            // Set the position of the sprites
            fallingSprite.SetPosition(position);
            splatSprite.SetPosition(position);
        }

        /// <summary>
        /// A publically accessible property to get the poop's position.
        /// </summary>
        public Vector2 Position => position;

        /// <summary>
        /// Attach the poop to a character.
        /// </summary>
        /// <param name="character">The character to attach to.</param>
        /// <param name="offset">The offset from the character to draw from.</param>
        public void AttachToCharacter(RoamingCharacter character, Vector2 offset)
        {
            attachedCharacter = character;
            attachedOffset = offset;
        }

        /// <summary>
        /// Update the poop.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public void Update(GameTime gameTime)
        {
            // If the poop is falling
            if (falling)
            {
                // Update the falling sprite
                fallingSprite.Update(gameTime);

                // If the falling sprite is at the end
                if (fallingSprite.IsAnimationAtEnd())
                {
                    // Play the splat sound
                    AudioManager.PlaySoundEffect("Splat");

                    // Fire the poop landed event
                    EventManager.FireEvent(KnownEvents.PoopLanded, this);

                    // Set the poop so it is no longer falling
                    falling = false;
                }
            }
            // If the poop is not falling and is attached to a character
            else if (attachedCharacter != null)
            {
                // Update the position / rotation of the poop based on the character's
                // position and the offset
                position = attachedCharacter.Position + attachedOffset;
                attachedRotation = attachedCharacter.Rotation;
                splatSprite.SetPosition(position);
                splatSprite.SetRotation(attachedRotation);
            }
        }

        /// <summary>
        /// Draw the poop.
        /// </summary>
        public void Draw()
        {
            // If the poop is falling
            if (falling)
            {
                // Draw the falling sprite
                fallingSprite.Draw();
            }
            // If the poop is not falling
            else
            {
                // Draw the splatted sprite
                splatSprite.Draw();
            }
        }
    }
}
