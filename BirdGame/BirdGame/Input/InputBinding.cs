namespace BirdGame.Input
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// An input binding.
    /// </summary>
    internal class InputBinding
    {
        /// <summary>
        /// The default keyboard key.
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// An alternative keyboard key.
        /// </summary>
        public Keys AltKey { get; set; }

        /// <summary>
        /// The default gamepad button.
        /// </summary>
        public Buttons Button { get; set; }

        /// <summary>
        /// An alternative gamepad button.
        /// </summary>
        public Buttons AltButton { get; set; }
    }
}
