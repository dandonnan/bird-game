namespace BirdGame.Characters
{
    using BirdGame.AI;
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.UI;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A character that roams around.
    /// </summary>
    internal class RoamingCharacter : AbstractCharacter
    {
        /// <summary>
        /// The speed to move by.
        /// </summary>
        private const float movementSpeed = 0.25f;

        /// <summary>
        /// The walking sprite.
        /// </summary>
        private readonly AnimatedSprite walkSprite;

        /// <summary>
        /// The walking with an item sprite.
        /// </summary>
        private readonly AnimatedSprite walkWithItemSprite;

        /// <summary>
        /// The item the character is holding.
        /// </summary>
        private readonly HeldItem heldItem;

        /// <summary>
        /// The list of nodes the character moves between.
        /// </summary>
        private List<Node> nodes;

        /// <summary>
        /// The index of the current node the character is at.
        /// </summary>
        private int currentNode;

        /// <summary>
        /// The bounds for the held item.
        /// </summary>
        private Rectangle heldItemBounds;

        /// <summary>
        /// The bounds for the character's head.
        /// </summary>
        private Rectangle headBounds;

        /// <summary>
        /// The bounds for the character.
        /// </summary>
        private Rectangle bounds;

        /// <summary>
        /// Whether the character has an item.
        /// </summary>
        private bool hasItem;

        /// <summary>
        /// Whether the character has vacated a spawn point.
        /// </summary>
        private bool vacatedSpawnPoint;

        /// <summary>
        /// A constructor for the roaming character.
        /// </summary>
        /// <param name="spawnPoint">The spawn point.</param>
        /// <param name="nodes">The list of nodes to move between.</param>
        public RoamingCharacter(SpawnPoint spawnPoint, List<Node> nodes)
        {
            this.spawnPoint = spawnPoint;
            this.nodes = nodes;
            currentNode = 0;

            // The minimum lifetime of the character, so they will not despawn
            // before this point (10 seconds)
            MinLifetime = 10000;

            // Get a random id to determine which sprite to use
            int id = new Random().Next(1, 7);

            // Get the sprites from the library
            walkSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}");
            walkWithItemSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}_Carry");

            // Change the speed the sprites update at
            walkSprite.SetFrameSpeed(300);
            walkWithItemSprite.SetFrameSpeed(300);

            vacatedSpawnPoint = false;

            origin = new Vector2(walkSprite.GetWidth() / 2, walkSprite.GetHeight() / 2);

            // Set the position, rotation and origin
            position = spawnPoint.Position;
            walkSprite.SetPosition(position);
            walkWithItemSprite.SetPosition(position);
            walkSprite.SetRotation(rotation);
            walkWithItemSprite.SetRotation(rotation);
            walkSprite.SetOrigin(origin);
            walkWithItemSprite.SetOrigin(origin);

            // Get a random number between 0 and 5, and if greater or equal to 3
            if (new Random().Next(0, 5) >= 3)
            {
                // The character should hold an item
                hasItem = true;

                // Get a random held item
                heldItem = new HeldItem((Item)new Random().Next(0, 3));
            }
        }

        /// <summary>
        /// Get the width of the character.
        /// </summary>
        /// <returns>The width.</returns>
        public override int GetWidth()
        {
            return walkSprite.GetWidth();
        }

        /// <summary>
        /// Get the height of the character.
        /// </summary>
        /// <returns>The height.</returns>
        public override int GetHeight()
        {
            return walkSprite.GetHeight();
        }

        /// <summary>
        /// Kill / despawn the character.
        /// </summary>
        public override void Kill()
        {
            // If the character has a spawn point
            if (spawnPoint != null)
            {
                // Vacate the spawn point
                spawnPoint.Vacate();
            }
        }

        /// <summary>
        /// Update the character.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public override void Update(GameTime gameTime)
        {
            // Move the character
            Move();

            // Update the sprite
            UpdateSprite(gameTime);

            // Update the bounds
            UpdateBounds();

            // Increase the time the character has been active
            Lifetime += gameTime.ElapsedGameTime.Milliseconds;

            // If the spawn point has not been vacated and the character has been
            // active for more than 1 second
            if (vacatedSpawnPoint == false && Lifetime > 1000)
            {
                // Vacate the spawn point
                vacatedSpawnPoint = true;
                spawnPoint.Vacate();
            }

            // If the character has an item
            if (hasItem)
            {
                // If the poop has landed inside the held item bounds
                if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, heldItemBounds, out Poop itemPoop))
                {
                    // Attach the poop to the character at the held item bounds
                    itemPoop.AttachToCharacter(this, new Vector2(heldItemBounds.X, heldItemBounds.Y));

                    // Increase the score by the held item's poop score
                    ScoreCounter.Add(heldItem.GetPoopTarget());
                }

                // If the bird is diving and in the bounds of the character
                if (WorldManager.GameWorld.Bird.State == BirdState.DivingUp
                    && position.X + 8 > WorldManager.GameWorld.Bird.Position.X
                    && position.X - 8 < WorldManager.GameWorld.Bird.Position.X + GetWidth()
                    && position.Y + 8 > WorldManager.GameWorld.Bird.Position.Y
                    && position.Y - 8 < WorldManager.GameWorld.Bird.Position.Y + GetHeight())
                {
                    // Take the item off the character
                    hasItem = false;

                    // Increase the score by the held item's dive score
                    ScoreCounter.Add(heldItem.GetDiveTarget());
                }
            }

            // If the poop has landed in the bounds of the character's head
            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, headBounds, out Poop poop))
            {
                // Increase the score by the character's head poop score
                ScoreCounter.Add(Target.PoopHead);

                // Attach the poop to the character
                poop.AttachToCharacter(this, new Vector2(headBounds.X, headBounds.Y));
            }

            // If the poop has landed in the bounds of the character
            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, bounds, out poop))
            {
                // Increase the score by the character's poop score
                ScoreCounter.Add(Target.PoopJacket);

                // Attach the poop to the character
                poop.AttachToCharacter(this, new Vector2(position.X, position.Y));
            }
        }

        /// <summary>
        /// Draw the character.
        /// </summary>
        public override void Draw()
        {
            // If the character has an item
            if (hasItem)
            {
                // Draw the walk with item sprite, and the item sprite
                walkWithItemSprite.Draw();
                heldItem.Draw();
            }
            // If the character does not have an item
            else
            {
                // Draw the walk sprite
                walkSprite.Draw();
            }
        }

        /// <summary>
        /// Move the character.
        /// </summary>
        private void Move()
        {
            // If the current node is in the node list, and the character is in range of a node
            if (currentNode < nodes.Count - 1 && InRangeOfNode())
            {
                // Move to the next node
                currentNode++;

                // If the next node is not in the list
                if (currentNode >= nodes.Count - 1)
                {
                    // Reset to the start of the node list
                    currentNode = 0;

                    // Get a new list of nodes starting at the current node
                    nodes = NodeNetwork.GetRouteFromNode(nodes[currentNode]);
                }
            }

            // Get the direction the character is facing
            Vector2 direction = nodes[currentNode].Position - position;
            direction.Normalize();

            // Set the rotation based on the direction
            rotation = (float)Math.Atan2(-direction.X, direction.Y) - 90;

            // Update the position based on the direction
            position += direction * movementSpeed;

            // If the character has an item
            if (hasItem)
            {
                // Update the item's position and rotation
                heldItem.SetPosition(position);
                heldItem.SetRotation(rotation);

                // Update the walk with item sprite's position and rotation
                walkWithItemSprite.SetPosition(position);
                walkWithItemSprite.SetRotation(rotation);
            }
            // If the character does not have an item
            else
            {
                // Update the walk sprite's position and rotation
                walkSprite.SetPosition(position);
                walkSprite.SetRotation(rotation);
            }
        }

        /// <summary>
        /// If the character is in range of a node.
        /// </summary>
        /// <returns>true if the character is in range of a node, false if not.</returns>
        private bool InRangeOfNode()
        {
            bool inRange = false;

            if (position.X > nodes[currentNode].Position.X - 4
                && position.X < nodes[currentNode].Position.X + 4
                && position.Y > nodes[currentNode].Position.Y - 4
                && position.Y < nodes[currentNode].Position.Y + 4)
            {
                inRange = true;
            }

            return inRange;
        }

        /// <summary>
        /// Update the bounds. These are just random number that need to work and need to be
        /// more accurate.
        /// </summary>
        private void UpdateBounds()
        {
            // If the character has an item
            if (hasItem)
            {
                // Set the bounds based on the current position
                heldItemBounds = new Rectangle((int)position.X, (int)position.Y, 8, 8);
            }

            // Set the bounds based on the current position
            headBounds = new Rectangle((int)(position.X + (GetWidth() / 2)) - 4, (int)(position.Y + (GetHeight() / 2)) - 4, 8, 8);
            bounds = new Rectangle((int)position.X, (int)position.Y, GetWidth(), GetHeight());
        }

        /// <summary>
        /// Update the sprite.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdateSprite(GameTime gameTime)
        {
            // If the character has an item
            if (hasItem)
            {
                // Update the with item sprite
                walkWithItemSprite.Update(gameTime);
            }
            // If the character does not have an item
            else
            {
                // Update the walk sprite
                walkSprite.Update(gameTime);
            }
        }
    }
}
