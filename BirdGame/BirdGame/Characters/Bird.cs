namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Graphics;
    using BirdGame.Input;
    using Microsoft.Xna.Framework;
    using System;

    internal class Bird : AbstractCharacter
    {
        private const float rotateSpeed = 0.025f;

        private BirdState state;

        private AnimatedSprite flyingSprite;

        private AnimatedSprite divingDownSprite;

        private AnimatedSprite divingUpSprite;

        private AnimatedSprite poopingSprite;

        private AnimatedSprite deadSprite;

        private Vector2 position;

        private float rotation;

        private bool canMove;

        public Bird()
        {
            // TODO: call Reset once all anims are in
            state = BirdState.Spawning;
            canMove = false;

            // TODO: get sprites
            flyingSprite = SpriteLibrary.GetAnimatedSprite("BirdFly");

            SetOrigin();

            SetPosition(new Vector2(-50, 300));
        }

        public void Reset()
        {
            ResetAllAnimations();
            state = BirdState.Spawning;
            canMove = false;
        }

        public void AllowMovement(bool movement)
        {
            canMove = movement;
        }

        public void AllowControl()
        {
            if (state == BirdState.Spawning)
            {
                state = BirdState.Flying;
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case BirdState.Spawning:
                    SpawnUpdate(gameTime);
                    break;

                case BirdState.Flying:
                    FlyUpdate(gameTime);
                    break;

                case BirdState.DivingDown:
                    DiveDownUpdate(gameTime);
                    break;

                case BirdState.DivingUp:
                    DiveUpUpdate(gameTime);
                    break;

                case BirdState.Pooping:
                    PoopUpdate(gameTime);
                    break;

                case BirdState.Dead:
                    DeadUpdate(gameTime);
                    break;
            }
        }

        public override void Draw()
        {
            switch (state)
            {
                case BirdState.Spawning:
                case BirdState.Flying:
                    flyingSprite.Draw();
                    break;

                case BirdState.DivingDown:
                    divingDownSprite.Draw();
                    break;

                case BirdState.DivingUp:
                    divingUpSprite.Draw();
                    break;

                case BirdState.Pooping:
                    poopingSprite.Draw();
                    break;

                case BirdState.Dead:
                    deadSprite.Draw();
                    break;
            }
        }

        private void SetOrigin()
        {
            Vector2 origin = new Vector2(flyingSprite.GetWidth() / 2, flyingSprite.GetHeight() / 2);

            flyingSprite.SetOrigin(origin);
            // todo: set more origins
        }

        private void SetRotation(float rotation)
        {
            this.rotation = rotation;

            flyingSprite.SetRotation(rotation);
            // todo: set more rotations
        }

        private void SetPosition(Vector2 position)
        {
            this.position = position;

            flyingSprite.SetPosition(position);
            // todo: set more positions;
        }

        private void SpawnUpdate(GameTime gameTime)
        {
            Move();

            flyingSprite.Update(gameTime);
        }

        private void FlyUpdate(GameTime gameTime)
        {
            Move();

            flyingSprite.Update(gameTime);

            if (InputManager.IsBindingPressed(DefaultBindings.Left) || InputManager.IsBindingHeld(DefaultBindings.Left))
            {
                SetRotation(rotation - rotateSpeed);
            }

            if (InputManager.IsBindingPressed(DefaultBindings.Right) || InputManager.IsBindingHeld(DefaultBindings.Right))
            {
                SetRotation(rotation + rotateSpeed);
            }

            if (InputManager.IsBindingPressed(DefaultBindings.Dive))
            {
                SwapToState(BirdState.DivingDown);
            }

            if (InputManager.IsBindingPressed(DefaultBindings.Poop))
            {
                SwapToState(BirdState.Pooping);
            }
        }

        private void Move()
        {
            if (canMove)
            {
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

                direction.Normalize();

                SetPosition(position + direction);
            }
        }

        private void DiveDownUpdate(GameTime gameTime)
        {
            SwapToStateOnAnimationEnd(divingDownSprite, BirdState.DivingUp);
        }

        private void DiveUpUpdate(GameTime gameTime)
        {
            SwapToStateOnAnimationEnd(divingUpSprite, BirdState.Flying);
        }

        private void PoopUpdate(GameTime gameTime)
        {
            SwapToStateOnAnimationEnd(poopingSprite, BirdState.Flying);
        }

        private void DeadUpdate(GameTime gameTime)
        {

        }

        private void ResetAllAnimations()
        {
            flyingSprite.Reset();
            divingDownSprite.Reset();
            divingUpSprite.Reset();
            poopingSprite.Reset();
            deadSprite.Reset();
        }

        private void SwapToState(BirdState newState)
        {
            state = newState;
            ResetAllAnimations();
        }

        private void SwapToStateOnAnimationEnd(AnimatedSprite sprite, BirdState newState)
        {
            if (sprite.IsAnimationAtEnd())
            {
                SwapToState(newState);
            }
        }
    }
}
