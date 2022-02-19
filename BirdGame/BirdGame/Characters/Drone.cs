namespace BirdGame.Characters
{
    using BirdGame.Audio;
    using BirdGame.Enums;
    using BirdGame.Graphics;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;

    /// <summary>
    /// The drone.
    /// </summary>
    internal class Drone : AbstractCharacter
    {
        /// <summary>
        /// The speed the drone moves at.
        /// </summary>
        private const float movementSpeed = 0.5f;

        /// <summary>
        /// The drone's idle sprite.
        /// </summary>
        private readonly AnimatedSprite idleSprite;

        /// <summary>
        /// Whether the drone is on screen.
        /// </summary>
        private bool onScreen;

        /// <summary>
        /// Whether the drone has left / vacated the spawn point.
        /// </summary>
        private bool vacated;

        /// <summary>
        /// The constructor for a drone.
        /// </summary>
        /// <param name="spawnPoint">The spawn point to spawn from.</param>
        public Drone(SpawnPoint spawnPoint)
        {
            // Set the minimum lifetime below 0 so it never despawns
            MinLifetime = -1;

            // The drone has not yet vacated the spawn point
            vacated = false;

            this.spawnPoint = spawnPoint;

            position = spawnPoint.Position;
            rotation = 0;
            idleSprite = SpriteLibrary.GetAnimatedSprite("DroneFly");

            origin = new Vector2(idleSprite.GetWidth() / 2, idleSprite.GetHeight() / 2);

            idleSprite.SetOrigin(origin);
            idleSprite.SetRotation(rotation);
            idleSprite.SetPosition(position);
            idleSprite.SetFrameSpeed(25);
        }

        /// <summary>
        /// Get the height of the drone.
        /// </summary>
        /// <returns>The height of the drone.</returns>
        public override int GetHeight()
        {
            return idleSprite.GetHeight();
        }

        /// <summary>
        /// Get the width of the drone.
        /// </summary>
        /// <returns>The width of the drone.</returns>
        public override int GetWidth()
        {
            return idleSprite.GetWidth();
        }

        /// <summary>
        /// The kill event. While other characters do things when they
        /// are killed / deactivated, drones don't do anything.
        /// </summary>
        public override void Kill()
        {
        }

        /// <summary>
        /// Update the drone.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
            // If the drone has not vacated the spawn point
            if (vacated == false)
            {
                // Vacate the spawn point
                spawnPoint.Vacate();
                vacated = true;

                // Increases the drone's lifetime - don't think this serves
                // any purpose anymore
                Lifetime += gameTime.ElapsedGameTime.Milliseconds;
            }

            // Update the sprite so it animates
            idleSprite.Update(gameTime);

            // Check that the drone is in the camera bounds
            CheckIfInBounds();

            // If the bird is not dead
            if (WorldManager.GameWorld.Bird.State != BirdState.Dead)
            {
                // Follow the bird
                FollowBird();
            }
            // If the bird is dead
            else
            {
                // Fly away
                FlyOff();
            }
        }

        /// <summary>
        /// Draw the drone.
        /// </summary>
        public override void Draw()
        {
            // If the drone is on screen
            if (onScreen)
            {
                // Draw the sprite
                idleSprite.Draw();
            }
        }

        /// <summary>
        /// Check if the drone is in the camera bounds.
        /// </summary>
        private void CheckIfInBounds()
        {
            // Get whether the drone is in the camera bounds / on screen
            bool inCameraBounds = WorldManager.GameWorld.Camera.CharacterInCameraBounds(this);

            // If the drone was on screen but is not any more
            if (onScreen && inCameraBounds == false)
            {
                // Mark the drone as off screen
                onScreen = false;

                // Stop the drone sound effects, and play a sound for leaving
                AudioManager.StopLoopingSoundEffect("DroneFly");
                AudioManager.PlaySoundEffect("DroneLeave");
            }

            // If the drone was not on screen but is now
            if (onScreen == false && inCameraBounds)
            {
                // Mark the drone as on screen
                onScreen = true;

                // Play a sound effect
                AudioManager.PlaySoundEffect("DroneEnter");
            }

            // If the drone was on screen, is in the camera bounds, and the flying sound
            // is not playing
            if (onScreen && inCameraBounds && AudioManager.IsLoopingSoundPlaying("DroneFly") == false)
            {
                // Play the looping drone sound
                AudioManager.PlayLoopingSoundEffect("DroneFly");
            }
        }

        /// <summary>
        /// Set the position of the drone.
        /// </summary>
        /// <param name="position">The position.</param>
        private void SetPosition(Vector2 position)
        {
            this.position = position;

            // Update the sprite position
            idleSprite.SetPosition(position);
        }

        /// <summary>
        /// Set the rotation of the drone.
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        private void SetRotation(float rotation)
        {
            this.rotation = rotation;

            // Update the sprite rotation
            idleSprite.SetRotation(rotation);
        }

        /// <summary>
        /// Follow the bird.
        /// </summary>
        private void FollowBird()
        {
            // Fly towards the bird
            FlyToTarget(WorldManager.GameWorld.Bird.Position);

            // If the bird is within the drone's bounds
            if (WorldManager.GameWorld.Bird.Position.X >= position.X
                && WorldManager.GameWorld.Bird.Position.X <= position.X + GetWidth()
                && WorldManager.GameWorld.Bird.Position.Y >= position.Y
                && WorldManager.GameWorld.Bird.Position.Y <= position.Y + GetHeight())
            {
                // Kill the bird
                WorldManager.GameWorld.Bird.Kill();
            }
        }

        /// <summary>
        /// Fly away.
        /// </summary>
        private void FlyOff()
        {
            // Fly towards the top left corner of the world
            FlyToTarget(Vector2.Zero);
        }

        /// <summary>
        /// Fly towards a target.
        /// </summary>
        /// <param name="target">The target to fly towards.</param>
        private void FlyToTarget(Vector2 target)
        {
            // Get the direction the target is in based on the current position
            Vector2 direction = target - position;
            direction.Normalize();

            // Get the angle to rotate to face
            float angle = (float)Math.Atan2(-direction.X, direction.Y) + 90;

            // Set the position and rotation of the drone to move towards the target
            SetPosition(position + (direction * movementSpeed));
            SetRotation(angle);
        }
    }
}
