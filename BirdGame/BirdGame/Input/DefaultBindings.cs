namespace BirdGame.Input
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A list of default bindings for inputs to handle
    /// the controls.
    /// </summary>
    internal class DefaultBindings
    {
        /// <summary>
        /// The up controls.
        /// </summary>
        public static InputBinding Up = new InputBinding
        {
            Key = Keys.W,
            AltKey = Keys.Up,
            Button = Buttons.LeftThumbstickUp,
            AltButton = Buttons.DPadUp
        };

        /// <summary>
        /// The down controls.
        /// </summary>
        public static InputBinding Down = new InputBinding
        {
            Key = Keys.S,
            AltKey = Keys.Down,
            Button = Buttons.LeftThumbstickDown,
            AltButton = Buttons.DPadDown
        };

        /// <summary>
        /// The left controls.
        /// </summary>
        public static InputBinding Left = new InputBinding
        {
            Key = Keys.A,
            AltKey = Keys.Left,
            Button = Buttons.LeftThumbstickLeft,
            AltButton = Buttons.DPadLeft
        };

        /// <summary>
        /// The right controls.
        /// </summary>
        public static InputBinding Right = new InputBinding
        {
            Key = Keys.D,
            AltKey = Keys.Right,
            Button = Buttons.LeftThumbstickRight,
            AltButton = Buttons.DPadRight
        };

        /// <summary>
        /// The pause controls.
        /// </summary>
        public static InputBinding Pause = new InputBinding
        {
            Key = Keys.Escape,
            AltKey = Keys.Back,
            Button = Buttons.Start,
            AltButton = Buttons.Back
        };

        /// <summary>
        /// The dive controls.
        /// </summary>
        public static InputBinding Dive = new InputBinding
        {
            Key = Keys.Space,
            AltKey = Keys.Enter,
            Button = Buttons.LeftTrigger,
            AltButton = Buttons.A
        };

        /// <summary>
        /// The poop controls.
        /// </summary>
        public static InputBinding Poop = new InputBinding
        {
            Key = Keys.LeftShift,
            AltKey = Keys.RightShift,
            Button = Buttons.RightTrigger,
            AltButton = Buttons.B
        };
    }
}
