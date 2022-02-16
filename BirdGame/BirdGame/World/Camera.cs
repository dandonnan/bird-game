namespace BirdGame.World
{
    using BirdGame.Characters;
    using BirdGame.Events;
    using Microsoft.Xna.Framework;

    internal class Camera
    {
        public Matrix Transform { get; private set; }

        private readonly Bird target;

        private readonly float speed;

        private readonly float scale;

        private Vector2 position;

        private Vector2 origin;

        private Vector2 center;

        private bool followTarget;

        public Camera(Bird bird)
        {
            UpdateViewport();

            target = bird;
            followTarget = false;

            scale = 1;
            speed = 1.25f;

            position = new Vector2(300, 640);
        }

        public void UpdateViewport()
        {
            float viewportWidth = WorldManager.SpriteBatch.GraphicsDevice.Viewport.Width;
            float viewportHeight = WorldManager.SpriteBatch.GraphicsDevice.Viewport.Height;

            center = new Vector2(viewportWidth / 2, viewportHeight / 2);
        }

        public void Update(GameTime gameTime)
        {
            if (EventManager.EventFired(KnownEvents.ResolutionChanged))
            {
                UpdateViewport();
            }

            Transform = Matrix.Identity * Matrix.CreateTranslation(-position.X, -position.Y, 0)
                        * Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(origin.X, origin.Y, 0)
                        * Matrix.CreateScale(new Vector3(scale, scale, scale));

            origin = center / scale;

            if (followTarget)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                position.X += (target.Position.X - position.X) * speed * delta;
                position.Y += (target.Position.Y - position.Y) * speed * delta;
            }
            else if (EventManager.EventFired(KnownEvents.SpawnBird))
            {
                followTarget = true;
            }
        }

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
