namespace BirdGame.World
{
    using BirdGame.Characters;
    using BirdGame.Events;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The camera.
    /// </summary>
    internal class Camera
    {
        /// <summary>
        /// The transform of the camera, used for what is displayed on screen.
        /// </summary>
        public Matrix Transform { get; private set; }

        /// <summary>
        /// The start position.
        /// </summary>
        private static Vector2 startPosition = new Vector2(300, 640);

        /// <summary>
        /// The target for the camera to follow.
        /// </summary>
        private readonly Bird target;

        /// <summary>
        /// The speed to follow the target.
        /// </summary>
        private readonly float speed;

        /// <summary>
        /// The position of the camera.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The origin of the camera.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The center of the camera.
        /// </summary>
        private Vector2 center;

        /// <summary>
        /// Whether the camera is following the target.
        /// </summary>
        private bool followTarget;

        /// <summary>
        /// The scale on the x-axis.
        /// </summary>
        private float scaleX;

        /// <summary>
        /// The scale on the y-axis.
        /// </summary>
        private float scaleY;

        /// <summary>
        /// The constructor of the camera.
        /// </summary>
        /// <param name="bird">The bird.</param>
        public Camera(Bird bird)
        {
            target = bird;
            followTarget = false;

            UpdateViewport();

            speed = 1.25f;

            position = startPosition;
        }

        /// <summary>
        /// Reset the camera to the start.
        /// </summary>
        public void Reset()
        {
            position = startPosition;
        }

        /// <summary>
        /// Update the viewport.
        /// </summary>
        public void UpdateViewport()
        {
            // Get the current width and height based on the window
            float viewportWidth = WorldManager.SpriteBatch.GraphicsDevice.Viewport.Width;
            float viewportHeight = WorldManager.SpriteBatch.GraphicsDevice.Viewport.Height;

            // Set the center of the camera
            center = new Vector2((viewportWidth - target.Origin.X) / 2 , (viewportHeight - target.Origin.Y) / 2);

            // Set the scale to divide the current viewport by the base resolution
            scaleX = viewportHeight / MainGame.DefaultWidth;
            scaleY = viewportHeight / MainGame.DefaultHeight;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public void Update(GameTime gameTime)
        {
            // If the resolution changed event has been fired
            if (EventManager.EventFired(KnownEvents.ResolutionChanged))
            {
                // Update the viewport to adjust the resolution
                UpdateViewport();
            }

            // Set the transform of the matrix based on the current position, the origin and the scale
            Transform = Matrix.Identity
                        * Matrix.CreateTranslation(-position.X, -position.Y, 0)
                        * Matrix.CreateRotationZ(0)
                        * Matrix.CreateTranslation(origin.X / scaleX, origin.Y / scaleY, 0)
                        * Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));

            // Set the origin to be the center
            origin = center;

            // If the camera is following a target
            if (followTarget)
            {
                // Update the camera based on the frame-rate
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If the bird is not dead
                if (target.State != Enums.BirdState.Dead)
                {
                    // Update the position to follow the bird
                    position.X += (target.Position.X - position.X) * speed * delta;
                    position.Y += (target.Position.Y - position.Y) * speed * delta;
                }
            }
            // If the camera is not following a target and the bird has been spawned
            else if (EventManager.EventFired(KnownEvents.SpawnBird))
            {
                // Follow the target
                followTarget = true;
            }
        }

        /// <summary>
        /// Get whether a poop is in the camera bounds.
        /// </summary>
        /// <param name="poop">The poop.</param>
        /// <returns>true if the poop is within the bounds, false if not.</returns>
        public bool PoopInCameraBounds(Poop poop)
        {
            bool inBounds = true;

            if (poop.Position.X + 4 < position.X - origin.X ||
                poop.Position.X - 4 > position.X + origin.X)
            {
                inBounds = false;
            }

            if (poop.Position.Y + 4 < position.Y - origin.Y ||
                poop.Position.Y - 4 > position.Y + origin.Y)
            {
                inBounds = false;
            }

            return inBounds;
        }

        /// <summary>
        /// Get whether a spawn point is in the camera bounds.
        /// </summary>
        /// <param name="spawnPoint">The spawn point.</param>
        /// <returns>true if the spawn point is within the bounds, false if not.</returns>
        public bool SpawnPointInCameraBounds(SpawnPoint spawnPoint)
        {
            bool inBounds = true;

            if (spawnPoint.Position.X + 30 < position.X - (origin.X / scaleX) ||
                spawnPoint.Position.X - 30 > position.X + (origin.X / scaleX))
            {
                inBounds = false;
            }

            if (spawnPoint.Position.Y + 30 < position.Y - (origin.Y / scaleY) ||
                spawnPoint.Position.Y - 30 > position.Y + (origin.Y / scaleY))
            {
                inBounds = false;
            }

            return inBounds;
        }

        /// <summary>
        /// Get whether a character is in the camera bounds.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <returns>true if the character is within the bounds, false if not.</returns>
        public bool CharacterInCameraBounds(AbstractCharacter character)
        {
            bool inBounds = true;

            if (character.Position.X + character.GetWidth() < position.X - origin.X ||
                character.Position.X - character.Origin.X > position.X + origin.X)
            {
                inBounds = false;
            }

            if (character.Position.Y + character.GetHeight() < position.Y - origin.Y ||
                character.Position.Y - character.Origin.Y > position.Y + origin.Y)
            {
                inBounds = false;
            }

            return inBounds;
        }
    }
}
