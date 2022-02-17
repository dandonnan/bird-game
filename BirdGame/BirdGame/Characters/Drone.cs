namespace BirdGame.Characters
{
    using BirdGame.Audio;
    using BirdGame.Enums;
    using BirdGame.Graphics;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;

    internal class Drone : AbstractCharacter
    {
        private const float movementSpeed = 0.5f;

        private readonly AnimatedSprite idleSprite;

        private bool onScreen;

        private bool vacated;

        public Drone(SpawnPoint spawnPoint)
        {
            MinLifetime = -1;
            vacated = false;

            this.spawnPoint = spawnPoint;

            position = spawnPoint.Position;
            rotation = 0;
            idleSprite = SpriteLibrary.GetAnimatedSprite("DroneFly");
            idleSprite.SetDepth(3);

            origin = new Vector2(idleSprite.GetWidth() / 2, idleSprite.GetHeight() / 2);

            idleSprite.SetOrigin(origin);
            idleSprite.SetRotation(rotation);
            idleSprite.SetPosition(position);
            idleSprite.SetFrameSpeed(25);
        }

        public override int GetHeight()
        {
            return idleSprite.GetWidth();
        }

        public override int GetWidth()
        {
            return idleSprite.GetHeight();
        }

        public override void Kill()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (vacated == false)
            {
                spawnPoint.Vacate();
                vacated = true;
                Lifetime += gameTime.ElapsedGameTime.Milliseconds;
            }

            idleSprite.Update(gameTime);
            CheckIfInBounds();

            if (WorldManager.GameWorld.Bird.State != BirdState.Dead)
            {
                FollowBird();
            }
            else
            {
                FlyOff();
            }
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
            }

            if (onScreen && inCameraBounds && AudioManager.IsLoopingSoundPlaying("DroneFly") == false)
            {
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
            FlyToTarget(WorldManager.GameWorld.Bird.Position);

            if (WorldManager.GameWorld.Bird.Position.X >= position.X
                && WorldManager.GameWorld.Bird.Position.X <= position.X + GetWidth()
                && WorldManager.GameWorld.Bird.Position.Y >= position.Y
                && WorldManager.GameWorld.Bird.Position.Y <= position.Y + GetHeight())
            {
                WorldManager.GameWorld.Bird.Kill();
            }
        }

        private void FlyOff()
        {
            FlyToTarget(Vector2.Zero);
        }

        private void FlyToTarget(Vector2 target)
        {
            Vector2 direction = target - position;
            direction.Normalize();

            float angle = (float)Math.Atan2(-direction.X, direction.Y) + 90;

            SetPosition(position + (direction * movementSpeed));
            SetRotation(angle);
        }
    }
}
