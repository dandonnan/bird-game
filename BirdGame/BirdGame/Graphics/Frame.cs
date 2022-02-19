namespace BirdGame.Graphics
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A frame for animating sprites.
    /// </summary>
    internal class Frame
    {
        /// <summary>
        /// A rectangle indicating where on the texture the frame is.
        /// </summary>
        public Rectangle Rectangle => new Rectangle(x, y, width, height);

        /// <summary>
        /// The x position of the frame.
        /// </summary>
        private readonly int x;

        /// <summary>
        /// The y position of the frame.
        /// </summary>
        private readonly int y;

        /// <summary>
        /// The width of the frame.
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The height of the frame.
        /// </summary>
        private readonly int height;

        /// <summary>
        /// A constructor for a frame that is in a square.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="widthHeight">A shared width and height.</param>
        public Frame(int x, int y, int widthHeight)
            : this(x, y, widthHeight, widthHeight)
        {
        }

        /// <summary>
        /// A constructor for a frame that is in a rectangle.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        public Frame(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
