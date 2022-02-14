namespace BirdGame.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    internal class InputManager
    {
        private static InputManager inputManager;

        private KeyboardState keyState;

        private KeyboardState lastKeyState;

        private GamePadState padState;

        private GamePadState lastPadState;

        private InputManager()
        {
            inputManager = this;
        }

        public static InputManager Initialise()
        {
            if (inputManager == null)
            {
                new InputManager();
            }

            return inputManager;
        }

        public void Update()
        {
            lastKeyState = keyState;
            lastPadState = padState;

            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsBindingPressed(InputBinding binding)
        {
            return IsKeyDown(binding) || IsButtonDown(binding);
        }

        public static bool IsBindingHeld(InputBinding binding)
        {
            return IsKeyHeld(binding) || IsButtonHeld(binding);
        }

        private static bool IsKeyDown(InputBinding binding)
        {
            return (inputManager.keyState.IsKeyDown(binding.Key)
                || inputManager.keyState.IsKeyDown(binding.AltKey))
                && (inputManager.lastKeyState.IsKeyDown(binding.Key) == false
                && inputManager.lastKeyState.IsKeyDown(binding.AltKey) == false);
        }

        private static bool IsKeyHeld(InputBinding binding)
        {
            return (inputManager.keyState.IsKeyDown(binding.Key)
                || inputManager.keyState.IsKeyDown(binding.AltKey))
                && (inputManager.lastKeyState.IsKeyDown(binding.Key)
                || inputManager.lastKeyState.IsKeyDown(binding.AltKey));
        }

        private static bool IsButtonDown(InputBinding binding)
        {
            return (inputManager.padState.IsButtonDown(binding.Button)
                || inputManager.padState.IsButtonDown(binding.AltButton))
                && (inputManager.lastPadState.IsButtonDown(binding.Button) == false
                && inputManager.lastPadState.IsButtonDown(binding.AltButton) == false);
        }

        private static bool IsButtonHeld(InputBinding binding)
        {
            return (inputManager.padState.IsButtonDown(binding.Button)
                || inputManager.padState.IsButtonDown(binding.AltButton))
                && (inputManager.lastPadState.IsButtonDown(binding.Button)
                || inputManager.lastPadState.IsButtonDown(binding.AltButton));
        }
    }
}
