namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// An item held by a character.
    /// </summary>
    internal class HeldItem
    {
        /// <summary>
        /// The type of item.
        /// </summary>
        private readonly Item itemType;

        /// <summary>
        /// The item's sprite.
        /// </summary>
        private Sprite itemSprite;

        /// <summary>
        /// The item's sprite when it has been pooped on.
        /// </summary>
        private Sprite itemWithPoopSprite;

        /// <summary>
        /// Whether or not the item has been pooped on.
        /// </summary>
        private bool hasPoop;

        /// <summary>
        /// The constructor for a held item.
        /// </summary>
        /// <param name="itemType">The type of item.</param>
        public HeldItem(Item itemType)
        {
            this.itemType = itemType;

            // Setup the item sprites
            SetupSprites();
        }

        /// <summary>
        /// Get a dive target based on the item type.
        /// </summary>
        /// <returns>A dive target based on the item type.</returns>
        public Target GetDiveTarget()
        {
            Target target;

            switch (itemType)
            {
                case Item.IceCream:
                    target = Target.DiveIceCream;
                    break;

                case Item.Coffee:
                    target = Target.DiveCoffee;
                    break;

                case Item.Chips:
                    target = Target.DiveChips;
                    break;

                default:
                    target = Target.DiveIceCream;
                    break;
            }

            return target;
        }

        /// <summary>
        /// Get a poop target based on the item type.
        /// </summary>
        /// <returns>A poop target based on the item type.</returns>
        public Target GetPoopTarget()
        {
            Target target;

            switch (itemType)
            {
                case Item.IceCream:
                    target = Target.PoopIceCream;
                    break;

                case Item.Coffee:
                    target = Target.PoopCoffee;
                    break;

                case Item.Chips:
                    target = Target.PoopChips;
                    break;

                default:
                    target = Target.PoopIceCream;
                    break;
            }

            return target;
        }

        /// <summary>
        /// Set the item to have been pooped on.
        /// </summary>
        public void PoopedOn()
        {
            hasPoop = true;
        }

        /// <summary>
        /// Set the position of the item's sprites.
        /// </summary>
        /// <param name="position">The position.</param>
        public void SetPosition(Vector2 position)
        {
            itemWithPoopSprite.SetPosition(position);
            itemSprite.SetPosition(position);
        }

        /// <summary>
        /// Set the rotation of the item's sprites.
        /// </summary>
        /// <param name="rotation">The rotation.</param>
        public void SetRotation(float rotation)
        {
            itemWithPoopSprite.SetRotation(rotation);
            itemSprite.SetRotation(rotation);
        }

        /// <summary>
        /// Draw the sprites.
        /// </summary>
        public void Draw()
        {
            // If the item has poop
            if (hasPoop)
            {
                // Draw the with poop sprite
                itemWithPoopSprite.Draw();
            }
            // Otherwise draw the regular sprite
            else
            {
                itemSprite.Draw();
            }
        }

        /// <summary>
        /// Setup sprites based on the item type.
        /// </summary>
        private void SetupSprites()
        {
            switch (itemType)
            {
                case Item.Coffee:
                    itemSprite = SpriteLibrary.GetSprite("Coffee");
                    itemWithPoopSprite = SpriteLibrary.GetSprite("CoffeePoop");
                    break;

                case Item.Chips:
                    itemSprite = SpriteLibrary.GetSprite("Chips");
                    itemWithPoopSprite = SpriteLibrary.GetSprite("ChipsPoop");
                    break;

                case Item.IceCream:
                    itemSprite = SpriteLibrary.GetSprite("IceCream");
                    itemWithPoopSprite = SpriteLibrary.GetSprite("IceCreamPoop");
                    break;

                default:
                    break;
            }

            // Set an origin - this is probably not right, and just stuck in numbers
            // that seemed to work
            Vector2 origin = new Vector2(6, 8);

            itemSprite.SetOrigin(origin);
            itemWithPoopSprite.SetOrigin(origin);
        }
    }
}
