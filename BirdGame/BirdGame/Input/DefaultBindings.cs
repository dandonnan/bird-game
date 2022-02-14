namespace BirdGame.Input
{
    using Microsoft.Xna.Framework.Input;

    internal class DefaultBindings
    {
        public static InputBinding Up = new InputBinding
        {
            Key = Keys.W,
            AltKey = Keys.Up,
            Button = Buttons.LeftThumbstickUp,
            AltButton = Buttons.DPadUp
        };

        public static InputBinding Down = new InputBinding
        {
            Key = Keys.S,
            AltKey = Keys.Down,
            Button = Buttons.LeftThumbstickDown,
            AltButton = Buttons.DPadDown
        };

        public static InputBinding Left = new InputBinding
        {
            Key = Keys.A,
            AltKey = Keys.Left,
            Button = Buttons.LeftThumbstickLeft,
            AltButton = Buttons.DPadLeft
        };

        public static InputBinding Right = new InputBinding
        {
            Key = Keys.D,
            AltKey = Keys.Right,
            Button = Buttons.LeftThumbstickRight,
            AltButton = Buttons.DPadRight
        };

        public static InputBinding Pause = new InputBinding
        {
            Key = Keys.Escape,
            AltKey = Keys.Back,
            Button = Buttons.Start,
            AltButton = Buttons.Back
        };

        public static InputBinding Dive = new InputBinding
        {
            Key = Keys.Space,
            AltKey = Keys.Enter,
            Button = Buttons.LeftTrigger,
            AltButton = Buttons.A
        };

        public static InputBinding Poop = new InputBinding
        {
            Key = Keys.LeftShift,
            AltKey = Keys.RightShift,
            Button = Buttons.RightTrigger,
            AltButton = Buttons.B
        };
    }
}
