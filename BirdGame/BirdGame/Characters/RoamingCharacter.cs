namespace BirdGame.Characters
{
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;
    using System;

    internal class RoamingCharacter : AbstractCharacter
    {
        private readonly AnimatedSprite walkSprite;

        private readonly AnimatedSprite walkWithItemSprite;

        private bool hasItem;

        private Vector2 position;

        private float rotation;

        public RoamingCharacter()
        {
            int id = new Random().Next(1, 7);

            walkSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}");
            walkWithItemSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}_Carry");
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            UpdateSprite(gameTime);
        }

        public override void Draw()
        {
            if (hasItem)
            {
                walkWithItemSprite.Draw();
            }
            else
            {
                walkSprite.Draw();
            }
        }

        private void Move()
        {

        }

        private void UpdateSprite(GameTime gameTime)
        {
            if (hasItem)
            {
                walkWithItemSprite.Update(gameTime);
            }
            else
            {
                walkSprite.Update(gameTime);
            }
        }
    }
}
