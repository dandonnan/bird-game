namespace BirdGame.Characters
{
    using BirdGame.Audio;
    using BirdGame.Graphics;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;

    internal class Drone : AbstractCharacter
    {
        private const float rotateSpeed = 0.025f;

        private const float movementSpeed = 0.5f;

        private readonly AnimatedSprite idleSprite;

        private bool onScreen;

        private float rotation;

        public Drone(Vector2 position)
        {
            this.position = position;
            rotation = 0;
            idleSprite = SpriteLibrary.GetAnimatedSprite("DroneFly");

            origin = new Vector2(idleSprite.GetWidth() / 2, idleSprite.GetHeight() / 2);

            idleSprite.SetOrigin(origin);
            idleSprite.SetRotation(rotation);
            idleSprite.SetPosition(position);
        }

        public override int GetHeight()
        {
            return idleSprite.GetWidth();
        }

        public override int GetWidth()
        {
            return idleSprite.GetHeight();
        }

        public override void Update(GameTime gameTime)
        {
            idleSprite.Update(gameTime);
            CheckIfInBounds();
            FollowBird();
        }

        public override void Draw()
        {
            if (onScreen)
            {
                idleSprite.Draw();
            }
        }

        private void CheckIfInBounds()
        {
            bool inCameraBounds = WorldManager.GameWorld.Camera.CharacterInCameraBounds(this);

            if (onScreen && inCameraBounds == false)
            {
                onScreen = false;
                AudioManager.StopLoopingSoundEffect("DroneFly");
                AudioManager.PlaySoundEffect("DroneLeave");
            }

            if (onScreen == false && inCameraBounds)
            {
                onScreen = true;
                AudioManager.PlaySoundEffect("DroneEnter");
                AudioManager.PlayLoopingSoundEffect("DroneFly");
            }
        }

        private void SetPosition(Vector2 position)
        {
            this.position = position;
            idleSprite.SetPosition(position);
        }

        private void SetRotation(float rotation)
        {
            this.rotation = rotation;
            idleSprite.SetRotation(rotation);
        }

        private void FollowBird()
        {
            Vector2 direction = WorldManager.GameWorld.Bird.Position - position;
            direction.Normalize();

            float angle = (float)Math.Atan2(-direction.X, direction.Y) + 90;

            SetPosition(position + (direction * movementSpeed));
            SetRotation(angle);
        }
    }
}
