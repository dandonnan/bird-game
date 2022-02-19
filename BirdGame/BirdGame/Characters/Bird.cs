namespace BirdGame.Characters
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

    /// <summary>
    /// The bird character. This inherits from the abstract character.
    /// </summary>
    internal class Bird : AbstractCharacter
    {
        /// <summary>
        /// The speed that the bird rotates.
        /// </summary>
        private const float rotateSpeed = 0.025f;

        /// <summary>
        /// An offset for turning around. This moves the bird to wherever
        /// they were minus the offset.
        /// </summary>
        private const float turnAroundPositionOffset = 20;

        /// <summary>
        /// A list of sound effects to play when the bird flaps.
        /// </summary>
        private readonly List<SoundEffect> flappingSounds;

        /// <summary>
        /// The flying sprite.
        /// </summary>
        private readonly AnimatedSprite flyingSprite;

        /// <summary>
        /// The sprite for diving down.
        /// </summary>
        private readonly AnimatedSprite divingDownSprite;

        /// <summary>
        /// The sprite for diving back up.
        /// </summary>
        private readonly AnimatedSprite divingUpSprite;

        /// <summary>
        /// The sprite for pooping.
        /// </summary>
        private readonly AnimatedSprite poopingSprite;

        /// <summary>
        /// The sprite when dead.
        /// </summary>
        private readonly AnimatedSprite deadSprite;

        /// <summary>
        /// The current state of the bird.
        /// </summary>
        private BirdState state;

        /// <summary>
        /// Whether or not the bird can move.
        /// </summary>
        private bool canMove;

        /// <summary>
        /// Whether or not the bird has flapped.
        /// </summary>
        private bool flapped;

        /// <summary>
        /// Whether or not the bird is turning around.
        /// </summary>
        private bool turningAround;

        /// <summary>
        /// The rotation the bird should rotate to when
        /// turning around.
        /// </summary>
        private float targetRotation;

        /// <summary>
        /// The position the bird should move to when
        /// turning around.
        /// </summary>
        private Vector2 targetPosition;

        /// <summary>
        /// Whether or not the bird is dead.
        /// </summary>
        private bool dead;

        /// <summary>
        /// The bird constructor.
        /// </summary>
        public Bird()
        {
            state = BirdState.Spawning;
            canMove = false;
            turningAround = true;

            // Set the sprites by getting them from the library
            flyingSprite = SpriteLibrary.GetAnimatedSprite("BirdFly");
            divingDownSprite = SpriteLibrary.GetAnimatedSprite("BirdDiveDown");
            divingUpSprite = SpriteLibrary.GetAnimatedSprite("BirdDiveUp");
            poopingSprite = SpriteLibrary.GetAnimatedSprite("BirdPoop");
            deadSprite = SpriteLibrary.GetAnimatedSprite("BirdDead");

            Reset();

            // Set the flapping sounds by getting them from the library
            flappingSounds = new List<SoundEffect>
            {
                AudioLibrary.GetSoundEffect("WingsFlap1"),
                AudioLibrary.GetSoundEffect("WingsFlap2")
            };

            // Set the origins on the sprites
            SetOrigin();
        }

        /// <summary>
        /// The publically accessible state of the bird.
        /// </summary>
        public BirdState State => state;

        /// <summary>
        /// Reset variables when the bird has died.
        /// </summary>
        public void Reset(bool playedBefore = false)
        {
            state = BirdState.Spawning;
            ResetAllAnimations();
            flapped = false;
            SetPosition(new Vector2(-10, 640));
            SetRotation(0);
            targetRotation = 0;

            if (playedBefore == false)
            {
                targetPosition = new Vector2(200, 0);
            }
            else
            {
                SetPosition(new Vector2(200, 640));
                AllowControl();
            }

            dead = false;
        }

        /// <summary>
        /// Allow the bird to move.
        /// </summary>
        /// <param name="movement">Whether to allow the bird to move.</param>
        public void AllowMovement(bool movement)
        {
            canMove = movement;
        }

        /// <summary>
        /// Allow the bird to be controlled.
        /// </summary>
        public void AllowControl()
        {
            // Only change the state if the bird is spawning
            if (state == BirdState.Spawning)
            {
                state = BirdState.Flying;
            }
        }

        /// <summary>
        /// Kill the bird.
        /// </summary>
        public override void Kill()
        {
            // Only change the state if the bird is in the air
            if (state == BirdState.Flying || state == BirdState.Pooping)
            {
                state = BirdState.Dead;
            }
        }

        /// <summary>
        /// Get the width of the bird, using the flying sprite.
        /// </summary>
        /// <returns>The width of the bird.</returns>
        public override int GetWidth()
        {
            return flyingSprite.GetWidth();
        }

        /// <summary>
        /// Get the height of the bird, using the flying sprite.
        /// </summary>
        /// <returns>The height of the bird.</returns>
        public override int GetHeight()
        {
            return flyingSprite.GetHeight();
        }

        /// <summary>
        /// Update the bird.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
            // Call a function based on the current state
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

        /// <summary>
        /// Draw the bird.
        /// </summary>
        public override void Draw()
        {
            // Call a function based on the current state
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

        /// <summary>
        /// Set the origin of the sprites.
        /// </summary>
        private void SetOrigin()
        {
            // Set the origin to be in the center of the sprite so it rotates from
            // there, instead of the top left which it would do by default.
            origin = new Vector2(flyingSprite.GetWidth() / 2, flyingSprite.GetHeight() / 2);

            // Set the origin on all of the sprites, so they draw in the same place
            flyingSprite.SetOrigin(origin);
            divingDownSprite.SetOrigin(origin);
            divingUpSprite.SetOrigin(origin);
            poopingSprite.SetOrigin(origin);
            deadSprite.SetOrigin(origin);
        }

        /// <summary>
        /// Set the rotation of the bird.
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        private void SetRotation(float rotation)
        {
            this.rotation = rotation;

            // Set the rotation on each of the sprites
            flyingSprite.SetRotation(rotation);
            divingDownSprite.SetRotation(rotation);
            divingUpSprite.SetRotation(rotation);
            poopingSprite.SetRotation(rotation);
            deadSprite.SetRotation(rotation);
        }

        /// <summary>
        /// Set the position of the bird.
        /// </summary>
        /// <param name="position">The position.</param>
        private void SetPosition(Vector2 position)
        {
            this.position = position;

            // Set the position on each of the sprites so they draw
            // in the right place
            flyingSprite.SetPosition(position);
            divingDownSprite.SetPosition(position);
            divingUpSprite.SetPosition(position);
            poopingSprite.SetPosition(position);
            deadSprite.SetPosition(position);
        }

        /// <summary>
        /// Update the bird in the spawn state.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void SpawnUpdate(GameTime gameTime)
        {
            // Move the bird
            Move();

            // Update the flying sprite so it animates
            flyingSprite.Update(gameTime);
        }

        /// <summary>
        /// Update the bird in the flying state.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void FlyUpdate(GameTime gameTime)
        {
            // Move the bird
            Move();

            // Update the flying sprite so it animates
            flyingSprite.Update(gameTime);

            // Play sounds
            PlaySounds();

            // If the bird is not turning around
            if (turningAround == false)
            {
                // Check if the bird is out of bounds so it can be turned around
                CheckIfOutOfBounds();

                // If the left button is pressed, rotate left using the rotation speed
                if (InputManager.IsBindingPressed(DefaultBindings.Left) || InputManager.IsBindingHeld(DefaultBindings.Left))
                {
                    SetRotation(rotation - rotateSpeed);
                }

                // If the right button is pressed, rotate right using the rotation speed
                if (InputManager.IsBindingPressed(DefaultBindings.Right) || InputManager.IsBindingHeld(DefaultBindings.Right))
                {
                    SetRotation(rotation + rotateSpeed);
                }

                // If the dive button is pressed
                if (InputManager.IsBindingPressed(DefaultBindings.Dive))
                {
                    // Play the dive sound effect
                    AudioManager.PlaySoundEffect("Dive");

                    // Change to the dive down state
                    SwapToState(BirdState.DivingDown);
                }

                // If the poop button is pressed then change to the poop state
                if (InputManager.IsBindingPressed(DefaultBindings.Poop))
                {
                    SwapToState(BirdState.Pooping);
                }
            }
            else
            {
                // If the bird needs to turn around, then turn around
                TurnAround();
            }
        }

        /// <summary>
        /// Play sound effects.
        /// </summary>
        private void PlaySounds()
        {
            // If the flying sprite is on the final frame and it hasn't flapped yet
            if (flyingSprite.CurrentFrame == flyingSprite.LastFrame && flapped == false)
            {
                // Play the flapping sound
                PlayFlappingSound();
            }
            else if (flapped && flyingSprite.CurrentFrame != flyingSprite.LastFrame)
            {
                // If the flying sprite is not on the last frame and it has previously
                // flapped, reset the flap so it can do it again
                flapped = false;
            }
        }

        /// <summary>
        /// Play a flapping sound.
        /// </summary>
        private void PlayFlappingSound()
        {
            // Get the index of a random sound in the flapping sound list
            int index = new Random().Next(0, flappingSounds.Count);

            // Play the sound effect at the random index from the flapping sound list
            AudioManager.PlaySoundEffect(flappingSounds[index]);

            // Set the bird to have flapped, to prevent it playing the sound until it
            // gets reset
            flapped = true;
        }
        
        /// <summary>
        /// Check if the bird is out of bounds.
        /// </summary>
        private void CheckIfOutOfBounds()
        {
            // If the bird is on the left side of the world's bounds
            if (position.X < GameWorld.WorldBounds.X)
            {
                // Turn the bird around and set rotations and positions it
                // needs to move towards before control returns to the player
                turningAround = true;
                targetRotation = rotation + (float)Math.PI;
                targetPosition = new Vector2(position.X + turnAroundPositionOffset, 0);
            }
            // If the bird is on the top of the world's bounds
            else if (position.Y < GameWorld.WorldBounds.Y)
            {
                // Turn the bird around and set rotations and positions it
                // needs to move towards before control returns to the player
                turningAround = true;
                targetRotation = rotation + (float)Math.PI;
                targetPosition = new Vector2(0, position.Y + turnAroundPositionOffset);
            }
            // If the bird is on the right of the world's bounds
            else if (position.X > GameWorld.WorldBounds.Width)
            {
                // Turn the bird around and set rotations and positions it
                // needs to move towards before control returns to the player
                turningAround = true;
                targetRotation = rotation - (float)Math.PI;
                targetPosition = new Vector2(position.X - turnAroundPositionOffset, 0);
            }
            // If the bird is below the world's bounsd
            else if (position.Y > GameWorld.WorldBounds.Height)
            {
                // Turn the bird around and set rotations and positions it
                // needs to move towards before control returns to the player
                turningAround = true;
                targetRotation = rotation - (float)Math.PI;
                targetPosition = new Vector2(0, position.Y - turnAroundPositionOffset);
            }
        }

        /// <summary>
        /// Turn the bird around.
        /// </summary>
        private void TurnAround()
        {
            // If the bird has not rotated to near-enough the target rotation
            if (rotation > targetRotation - rotateSpeed
                && rotation < targetRotation + rotateSpeed)
            {
                // An offset so the bird can move near-enough to the target position
                int positionOffset = 4;

                // If the bird has a target position and it is to the left of the target
                if (targetPosition.X != 0 && position.X < targetPosition.X - (turnAroundPositionOffset / positionOffset))
                {
                    // Move the bird right
                    position.X++;
                }
                // If the bird has a target position and it is to the right of the target
                else if (targetPosition.X != 0 && position.X > targetPosition.X + (turnAroundPositionOffset / positionOffset))
                {
                    // Move the bird left
                    position.X--;
                }
                // If the bird has a target position and it is above the target
                else if (targetPosition.Y != 0 && position.Y < targetPosition.Y - (turnAroundPositionOffset / positionOffset))
                {
                    // Move the bird down
                    position.Y++;
                }
                // If the bird has a target position and it is below the target
                else if (targetPosition.Y != 0 && position.Y > targetPosition.Y + (turnAroundPositionOffset / positionOffset))
                {
                    // Move the bird up
                    position.Y--;
                }
                else
                {
                    // The bird is at the target, so stop it turning around
                    turningAround = false;
                }
            }
            // If the rotation is lower than the target, move it towards it
            else if (rotation < targetRotation)
            {
                SetRotation(rotation + rotateSpeed);
            }
            // If the rotation is above the target, move it towards it
            else if (rotation > targetRotation)
            {
                SetRotation(rotation - rotateSpeed);
            }
        }

        /// <summary>
        /// Move the bird.
        /// </summary>
        private void Move()
        {
            // If the bird can move
            if (canMove)
            {
                // Get the direction of the bird based on the rotation
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

                direction.Normalize();

                // Set the position of the bird based on the direction
                SetPosition(position + direction);
            }
        }

        /// <summary>
        /// Update the bird when it is diving down.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void DiveDownUpdate(GameTime gameTime)
        {
            // Update the diving down sprite so it animates
            divingDownSprite.Update(gameTime);

            // Swap the state to diving up when the diving down sprite reaches
            // the end of its animation
            SwapToStateOnAnimationEnd(divingDownSprite, BirdState.DivingUp);
        }

        /// <summary>
        /// Update the bird when it is diving up - and I've only just realised
        /// that diving up is not a real thing.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void DiveUpUpdate(GameTime gameTime)
        {
            // Update the diving up sprite so it animates
            divingUpSprite.Update(gameTime);

            // Swap the state to flying when the diving up sprite reaches
            // the end of its animation
            SwapToStateOnAnimationEnd(divingUpSprite, BirdState.Flying);
        }

        /// <summary>
        /// Update the bird when it is pooping.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void PoopUpdate(GameTime gameTime)
        {
            // Update the poop sprite so it animates
            poopingSprite.Update(gameTime);

            // Swap the state to flying when the poop sprite reaches
            // the end of its animation, and also play the poop sound
            // and trigger the poop spawned event
            SwapToStateOnAnimationEnd(poopingSprite, BirdState.Flying, "Poop", KnownEvents.PoopSpawned);
        }

        /// <summary>
        /// Update the bird when it is dead.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void DeadUpdate(GameTime gameTime)
        {
            // Update the dead sprite so it animates
            deadSprite.Update(gameTime);

            // If the bird is not marked as dead but it is at the animation's end
            if (deadSprite.IsAnimationAtEnd() && dead == false)
            {
                // Mark the bird as dead
                dead = true;
                
                // Trigger an event to let other objects know the bird is dead
                EventManager.FireEvent(KnownEvents.BirdDead);
            }
        }

        /// <summary>
        /// Reset all the animations.
        /// </summary>
        private void ResetAllAnimations()
        {
            flyingSprite.Reset();
            divingDownSprite.Reset();
            divingUpSprite.Reset();
            poopingSprite.Reset();
            deadSprite.Reset();
        }

        /// <summary>
        /// Swap the state of the bird.
        /// </summary>
        /// <param name="newState">The state to swap to.</param>
        private void SwapToState(BirdState newState)
        {
            // Set the state
            state = newState;
            
            // Reset all animations so they start from the first frame
            ResetAllAnimations();
        }

        /// <summary>
        /// Swap to a different state when an animation ends.
        /// </summary>
        /// <param name="sprite">The sprite of the animation to check when it ends.</param>
        /// <param name="newState">The state to swap to.</param>
        /// <param name="soundEffectId">(Optional) The id of the sound effect to play.</param>
        /// <param name="eventToTrigger">(Optional) The name of an event to trigger.</param>
        private void SwapToStateOnAnimationEnd(AnimatedSprite sprite, BirdState newState, string soundEffectId = null, string eventToTrigger = null)
        {
            // If the sprite is at the end of it's animation
            if (sprite.IsAnimationAtEnd())
            {
                // Swap to a different state
                SwapToState(newState);

                // If a sound effect id has been provided, then play the sound effect
                if (string.IsNullOrEmpty(soundEffectId) == false)
                {
                    AudioManager.PlaySoundEffect(soundEffectId);
                }

                // If an event id has been provided, then trigger the event
                if (string.IsNullOrEmpty(eventToTrigger) == false)
                {
                    EventManager.FireEvent(eventToTrigger);
                }
            }
        }
    }
}
