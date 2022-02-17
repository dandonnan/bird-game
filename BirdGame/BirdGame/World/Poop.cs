namespace BirdGame.World
{
    using BirdGame.Audio;
    using BirdGame.Characters;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;
    using System;

    internal class Poop
    {
        private readonly AnimatedSprite fallingSprite;

        private readonly Sprite splatSprite;

        private Vector2 position;

        private RoamingCharacter attachedCharacter;

        private Vector2 attachedOffset;

        private float attachedRotation;

        private bool falling;

        public Poop(Vector2 position)
        {
            this.position = position;
            falling = true;

            fallingSprite = SpriteLibrary.GetAnimatedSprite("PoopDrop");
            splatSprite = SpriteLibrary.GetSprite("StainPoop");

            fallingSprite.SetPosition(position);
            splatSprite.SetPosition(position);

            fallingSprite.SetDepth(2);
            splatSprite.SetDepth(0);
        }

        public Vector2 Position => position;

        public void AttachToCharacter(RoamingCharacter character, Vector2 offset)
        {
            attachedCharacter = character;
            attachedOffset = offset;
            splatSprite.SetDepth(2);
        }

        public void Update(GameTime gameTime)
        {
            if (falling)
            {
                fallingSprite.Update(gameTime);

                if (fallingSprite.IsAnimationAtEnd())
                {
                    AudioManager.PlaySoundEffect("Splat");
                    EventManager.FireEvent(KnownEvents.PoopLanded, this);
                    falling = false;
                }
            }
            else if (attachedCharacter != null)
            {
                position = attachedCharacter.Position + attachedOffset;
                attachedRotation = attachedCharacter.Rotation;
                splatSprite.SetPosition(position);
                splatSprite.SetRotation(attachedRotation);
            }
        }

        public void Draw()
        {
            if (falling)
            {
                fallingSprite.Draw();
            }
            else
            {
                splatSprite.Draw();
            }
        }
    }
}
