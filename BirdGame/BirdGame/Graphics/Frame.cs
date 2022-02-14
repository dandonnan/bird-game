namespace BirdGame.Graphics
{
    using Microsoft.Xna.Framework;

    internal class Frame
    {
        public Rectangle Rectangle => new Rectangle(x, y, width, height);

        private readonly int x;

        private readonly int y;

        private readonly int width;

        private readonly int height;

        public Frame(int x, int y, int widthHeight)
            : this(x, y, widthHeight, widthHeight)
        {
        }

        public Frame(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
