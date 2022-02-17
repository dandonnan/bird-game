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

    internal class RoamingCharacter : AbstractCharacter
    {
        private const float movementSpeed = 0.25f;

        private readonly AnimatedSprite walkSprite;

        private readonly AnimatedSprite walkWithItemSprite;

        private readonly HeldItem heldItem;

        private readonly List<Node> nodes;

        private int currentNode;

        private Rectangle heldItemBounds;

        private Rectangle headBounds;

        private Rectangle bounds;

        private bool hasItem;

        private bool vacatedSpawnPoint;

        public RoamingCharacter(SpawnPoint spawnPoint, List<Node> nodes)
        {
            this.spawnPoint = spawnPoint;
            this.nodes = nodes;
            currentNode = 0;

            MinLifetime = 10000;

            int id = new Random().Next(1, 7);

            walkSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}");
            walkWithItemSprite = SpriteLibrary.GetAnimatedSprite($"NPC{id}_Carry");

            walkSprite.SetFrameSpeed(300);
            walkWithItemSprite.SetFrameSpeed(300);

            vacatedSpawnPoint = false;

            origin = new Vector2(walkSprite.GetWidth() / 2, walkSprite.GetHeight() / 2);

            position = spawnPoint.Position;
            walkSprite.SetPosition(position);
            walkWithItemSprite.SetPosition(position);
            walkSprite.SetRotation(rotation);
            walkWithItemSprite.SetRotation(rotation);
            walkSprite.SetOrigin(origin);
            walkWithItemSprite.SetOrigin(origin);

            if (new Random().Next(0, 5) >= 3)
            {
                hasItem = true;
                heldItem = new HeldItem((Item)new Random().Next(0, 3));
            }
        }

        public override int GetWidth()
        {
            return walkSprite.GetWidth();
        }

        public override int GetHeight()
        {
            return walkSprite.GetHeight();
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
            Move();
            UpdateSprite(gameTime);
            UpdateBounds();

            Lifetime += gameTime.ElapsedGameTime.Milliseconds;

            if (vacatedSpawnPoint == false && Lifetime > 1000)
            {
                vacatedSpawnPoint = true;
                spawnPoint.Vacate();
            }

            if (hasItem)
            {
                if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, heldItemBounds, out Poop itemPoop))
                {
                    itemPoop.AttachToCharacter(this, new Vector2(heldItemBounds.X, heldItemBounds.Y));
                    ScoreCounter.Add(heldItem.GetPoopTarget());
                }

                if (WorldManager.GameWorld.Bird.State == BirdState.DivingUp
                    && position.X > WorldManager.GameWorld.Bird.Position.X
                    && position.X < WorldManager.GameWorld.Bird.Position.X + GetWidth()
                    && position.Y > WorldManager.GameWorld.Bird.Position.Y
                    && position.Y < WorldManager.GameWorld.Bird.Position.Y + GetHeight())
                {
                    hasItem = false;
                    ScoreCounter.Add(heldItem.GetDiveTarget());
                }
            }

            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, headBounds, out Poop poop))
            {
                ScoreCounter.Add(Target.PoopHead);
                poop.AttachToCharacter(this, new Vector2(headBounds.X, headBounds.Y));
            }

            if (EventManager.IsEventFiredInBounds(KnownEvents.PoopLanded, bounds, out poop))
            {
                ScoreCounter.Add(Target.PoopJacket);
                poop.AttachToCharacter(this, new Vector2(position.X, position.Y));
            }
        }

        public override void Draw()
        {
            if (hasItem)
            {
                walkWithItemSprite.Draw();
                heldItem.Draw();
            }
            else
            {
                walkSprite.Draw();
            }
        }

        private void Move()
        {
            if (currentNode < nodes.Count - 1 && InRangeOfNode())
            {
                currentNode++;
            }

            Vector2 direction = nodes[currentNode].Position - position;
            direction.Normalize();

            rotation = (float)Math.Atan2(-direction.X, direction.Y) - 90;

            position += direction * movementSpeed;

            if (hasItem)
            {
                heldItem.SetPosition(position);
                walkWithItemSprite.SetPosition(position);
                walkWithItemSprite.SetRotation(rotation);
            }
            else
            {
                walkSprite.SetPosition(position);
                walkSprite.SetRotation(rotation);
            }
        }

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

        private void UpdateBounds()
        {
            if (hasItem)
            {
                heldItemBounds = new Rectangle((int)position.X, (int)position.Y, 4, 4);
            }

            headBounds = new Rectangle((int)(position.X + (GetWidth() / 2)) - 4, (int)(position.Y + (GetHeight() / 2)) - 4, 8, 8);
            bounds = new Rectangle((int)position.X, (int)position.Y, GetWidth(), GetHeight());
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
