namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Graphics;
    using Microsoft.Xna.Framework;

    internal class HeldItem
    {
        private readonly Item itemType;

        private Sprite itemSprite;

        private Sprite itemWithPoopSprite;

        private bool hasPoop;

        public HeldItem(Item itemType)
        {
            this.itemType = itemType;
            SetupSprites();
        }

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

        public void PoopedOn()
        {
            hasPoop = true;
        }

        public void SetPosition(Vector2 position)
        {
            itemWithPoopSprite.SetPosition(position);
            itemSprite.SetPosition(position);
        }

        public void Draw()
        {
            if (hasPoop)
            {
                itemWithPoopSprite.Draw();
            }
            else
            {
                itemSprite.Draw();
            }
        }

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
            }
        }
    }
}
