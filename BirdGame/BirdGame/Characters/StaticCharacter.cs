namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.UI;
    using BirdGame.World;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A character that is static and does not move.
    /// This is hard-coded to be a car, but could be modified
    /// to have other static characters.
    /// </summary>
    internal class StaticCharacter : AbstractCharacter
    {
        /// <summary>
        /// The idle animation.
        /// </summary>
        private readonly Sprite idleAnimation;

        /// <summary>
        /// The bounds of the character.
        /// </summary>
        private readonly Rectangle bounds;

        /// <summary>
        /// The constructor for the character.
        /// </summary>
        /// <param name="spawnPoint">The spawn point.</param>
        public StaticCharacter(SpawnPoint spawnPoint)
        {
            // Make the character stay alive for 10 seconds
            MinLifetime = 10000;

            this.spawnPoint = spawnPoint;
            position = spawnPoint.Position;
            idleAnimation = SpriteLibrary.GetSprite("Car");
            idleAnimation.SetPosition(spawnPoint.Position);

            // Set the bounds for the character to be pooped on
            bounds = new Rectangle((int)spawnPoint.Position.X, (int)spawnPoint.Position.Y,
                    idleAnimation.GetWidth(), idleAnimation.GetHeight());
        }

        /// <summary>
        /// Get the width of the character.
        /// </summary>
        /// <returns>The width of the character.</returns>
        public override int GetWidth()
        {
            return idleAnimation.GetWidth();
        }

        /// <summary>
        /// Get the height of the character.
        /// </summary>
        /// <returns>The height of the character.</returns>
        public override int GetHeight()
        {
            return idleAnimation.GetHeight();
        }

        /// <summary>
        /// Kill / despawn the character.
        /// </summary>
        public override void Kill()
        {
            // If there was a spawn point
            if (spawnPoint != null)
            {
                // Vacate the spawn point so other characters can spawn there
                spawnPoint.Vacate();
            }
        }

        /// <summary>
        /// Update the character.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
            // Increase the lifetime of the character
            Lifetime += gameTime.ElapsedGameTime.Milliseconds;

            // If a poop landed event has been fired, and the poop is within the character's bounds
            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, bounds, out Poop _))
            {
                // Increase the score, as the poop has hit the character
                ScoreCounter.Add(Target.PoopCar);
            }
        }

        /// <summary>
        /// Draw the character.
        /// </summary>
        public override void Draw()
        {
            idleAnimation.Draw();
        }
    }
}
