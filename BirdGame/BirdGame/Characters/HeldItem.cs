namespace BirdGame.Characters
{
    using BirdGame.Enums;
    using BirdGame.Graphics;

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

        public void PoopedOn()
        {
            hasPoop = true;
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
