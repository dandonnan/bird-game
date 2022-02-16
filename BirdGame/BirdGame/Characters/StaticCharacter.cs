namespace BirdGame.Characters
{
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;

    internal class StaticCharacter : AbstractCharacter
    {
        private AnimatedSprite idleAnimation;

        public StaticCharacter()
        {

        }

        public override int GetWidth()
        {
            return idleAnimation.GetWidth();
        }

        public override int GetHeight()
        {
            return idleAnimation.GetHeight();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
        }
    }
}
