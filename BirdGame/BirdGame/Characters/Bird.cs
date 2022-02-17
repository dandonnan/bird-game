﻿namespace BirdGame.Characters
{
    using BirdGame.Audio;
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.Input;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using System;
    using System.Collections.Generic;

    internal class Bird : AbstractCharacter
    {
        private const float rotateSpeed = 0.025f;

        private const float turnAroundPositionOffset = 20;

        private readonly List<SoundEffect> flappingSounds;

        private BirdState state;

        private AnimatedSprite flyingSprite;

        private AnimatedSprite divingDownSprite;

        private AnimatedSprite divingUpSprite;

        private AnimatedSprite poopingSprite;

        private AnimatedSprite deadSprite;

        private bool canMove;

        private bool flapped;

        private bool turningAround;

        private float targetRotation;

        private Vector2 targetPosition;

        private bool dead;

        public Bird()
        {
            state = BirdState.Spawning;
            canMove = false;
            turningAround = true;
            targetRotation = 0;
            targetPosition = new Vector2(200, 0);

            flyingSprite = SpriteLibrary.GetAnimatedSprite("BirdFly");
            divingDownSprite = SpriteLibrary.GetAnimatedSprite("BirdDiveDown");
            divingUpSprite = SpriteLibrary.GetAnimatedSprite("BirdDiveUp");
            poopingSprite = SpriteLibrary.GetAnimatedSprite("BirdPoop");
            deadSprite = SpriteLibrary.GetAnimatedSprite("BirdDead");

            flyingSprite.SetDepth(3);
            divingDownSprite.SetDepth(3);
            divingUpSprite.SetDepth(3);
            poopingSprite.SetDepth(3);
            deadSprite.SetDepth(3);

            Reset();

            flappingSounds = new List<SoundEffect>
            {
                AudioLibrary.GetSoundEffect("WingsFlap1"),
                AudioLibrary.GetSoundEffect("WingsFlap2")
            };

            SetOrigin();
        }

        public BirdState State => state;

        public void Reset()
        {
            ResetAllAnimations();
            dead = false;
            flapped = false;
            state = BirdState.Flying;
            SetPosition(new Vector2(-10, 640));
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

        public override void Kill()
        {
            if (state == BirdState.Flying || state == BirdState.Pooping)
            {
                state = BirdState.Dead;
            }
        }

        public override int GetWidth()
        {
            return flyingSprite.GetWidth();
        }

        public override int GetHeight()
        {
            return flyingSprite.GetHeight();
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
            origin = new Vector2(flyingSprite.GetWidth() / 2, flyingSprite.GetHeight() / 2);

            flyingSprite.SetOrigin(origin);
            divingDownSprite.SetOrigin(origin);
            divingUpSprite.SetOrigin(origin);
            poopingSprite.SetOrigin(origin);
            deadSprite.SetOrigin(origin);
        }

        private void SetRotation(float rotation)
        {
            this.rotation = rotation;

            flyingSprite.SetRotation(rotation);
            divingDownSprite.SetRotation(rotation);
            divingUpSprite.SetRotation(rotation);
            poopingSprite.SetRotation(rotation);
            deadSprite.SetRotation(rotation);
        }

        private void SetPosition(Vector2 position)
        {
            this.position = position;

            flyingSprite.SetPosition(position);
            divingDownSprite.SetPosition(position);
            divingUpSprite.SetPosition(position);
            poopingSprite.SetPosition(position);
            deadSprite.SetPosition(position);
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

            PlaySounds();

            if (turningAround == false)
            {
                CheckIfOutOfBounds();

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
                    AudioManager.PlaySoundEffect("Dive");
                    SwapToState(BirdState.DivingDown);
                }

                if (InputManager.IsBindingPressed(DefaultBindings.Poop))
                {
                    SwapToState(BirdState.Pooping);
                }
            }
            else
            {
                TurnAround();
            }
        }

        private void PlaySounds()
        {
            if (flyingSprite.CurrentFrame == flyingSprite.LastFrame && flapped == false)
            {
                PlayFlappingSound();
            }
            else if (flapped && flyingSprite.CurrentFrame != flyingSprite.LastFrame)
            {
                flapped = false;
            }
        }

        private void PlayFlappingSound()
        {
            int index = new Random().Next(0, flappingSounds.Count);

            AudioManager.PlaySoundEffect(flappingSounds[index]);

            flapped = true;
        }
        
        private void CheckIfOutOfBounds()
        {
            if (position.X < GameWorld.WorldBounds.X)
            {
                turningAround = true;
                targetRotation = rotation + (float)Math.PI;
                targetPosition = new Vector2(position.X + turnAroundPositionOffset, 0);
            }
            else if (position.Y < GameWorld.WorldBounds.Y)
            {
                turningAround = true;
                targetRotation = rotation + (float)Math.PI;
                targetPosition = new Vector2(0, position.Y + turnAroundPositionOffset);
            }
            else if (position.X > GameWorld.WorldBounds.Width)
            {
                turningAround = true;
                targetRotation = rotation - (float)Math.PI;
                targetPosition = new Vector2(position.X - turnAroundPositionOffset, 0);
            }
            else if (position.Y > GameWorld.WorldBounds.Height)
            {
                turningAround = true;
                targetRotation = rotation - (float)Math.PI;
                targetPosition = new Vector2(0, position.Y - turnAroundPositionOffset);
            }
        }

        private void TurnAround()
        {
            if (rotation > targetRotation - rotateSpeed
                && rotation < targetRotation + rotateSpeed)
            {
                int positionOffset = 4;

                if (targetPosition.X != 0 && position.X < targetPosition.X - (turnAroundPositionOffset / positionOffset))
                {
                    position.X++;
                }
                else if (targetPosition.X != 0 && position.X > targetPosition.X + (turnAroundPositionOffset / positionOffset))
                {
                    position.X--;
                }
                else if (targetPosition.Y != 0 && position.Y < targetPosition.Y - (turnAroundPositionOffset / positionOffset))
                {
                    position.Y++;
                }
                else if (targetPosition.Y != 0 && position.Y > targetPosition.Y + (turnAroundPositionOffset / positionOffset))
                {
                    position.Y--;
                }
                else
                {
                    turningAround = false;
                }
            }
            else if (rotation < targetRotation)
            {
                SetRotation(rotation + rotateSpeed);
            }
            else if (rotation > targetRotation)
            {
                SetRotation(rotation - rotateSpeed);
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
            divingDownSprite.Update(gameTime);

            SwapToStateOnAnimationEnd(divingDownSprite, BirdState.DivingUp);
        }

        private void DiveUpUpdate(GameTime gameTime)
        {
            divingUpSprite.Update(gameTime);

            SwapToStateOnAnimationEnd(divingUpSprite, BirdState.Flying);
        }

        private void PoopUpdate(GameTime gameTime)
        {
            poopingSprite.Update(gameTime);

            SwapToStateOnAnimationEnd(poopingSprite, BirdState.Flying, "Poop", KnownEvents.PoopSpawned);
        }

        private void DeadUpdate(GameTime gameTime)
        {
            deadSprite.Update(gameTime);

            if (deadSprite.IsAnimationAtEnd() && dead == false)
            {
                dead = true;
                EventManager.FireEvent(KnownEvents.BirdDead);
            }
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

        private void SwapToStateOnAnimationEnd(AnimatedSprite sprite, BirdState newState, string soundEffectId = null, string eventToTrigger = null)
        {
            if (sprite.IsAnimationAtEnd())
            {
                SwapToState(newState);

                if (string.IsNullOrEmpty(soundEffectId) == false)
                {
                    AudioManager.PlaySoundEffect(soundEffectId);
                }

                if (string.IsNullOrEmpty(eventToTrigger) == false)
                {
                    EventManager.FireEvent(eventToTrigger);
                }
            }
        }
    }
}
