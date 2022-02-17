namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.UI;
    using BirdGame.World;
    using Microsoft.Xna.Framework;

    internal class StaticCharacter : AbstractCharacter
    {
        private readonly Sprite idleAnimation;

        private readonly Rectangle bounds;

        public StaticCharacter(SpawnPoint spawnPoint)
        {
            MinLifetime = 10000;

            this.spawnPoint = spawnPoint;
            position = spawnPoint.Position;
            idleAnimation = SpriteLibrary.GetSprite("Car");
            idleAnimation.SetPosition(spawnPoint.Position);
            bounds = new Rectangle((int)spawnPoint.Position.X, (int)spawnPoint.Position.Y,
                    idleAnimation.GetWidth(), idleAnimation.GetHeight());
        }

        public override int GetWidth()
        {
            return idleAnimation.GetWidth();
        }

        public override int GetHeight()
        {
            return idleAnimation.GetHeight();
        }

        public override void Kill()
        {
            if (spawnPoint != null)
            {
                spawnPoint.Vacate();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Lifetime += gameTime.ElapsedGameTime.Milliseconds;

            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, bounds, out Poop _))
            {
                ScoreCounter.Add(Target.PoopCar);
            }
        }

        public override void Draw()
        {
            idleAnimation.Draw();
        }
    }
}
