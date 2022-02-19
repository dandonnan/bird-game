namespace BirdGame.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A manager for input.
    /// </summary>
    internal class InputManager
    {
        /// <summary>
        /// A singleton for the input manager, so there can only ever be one.
        /// </summary>
        private static InputManager inputManager;

        /// <summary>
        /// The current keyboard state.
        /// </summary>
        private KeyboardState keyState;

        /// <summary>
        /// The keyboard state from the last frame.
        /// </summary>
        private KeyboardState lastKeyState;

        /// <summary>
        /// The current gamepad state.
        /// </summary>
        private GamePadState padState;

        /// <summary>
        /// The gamepad state from the last frame.
        /// </summary>
        private GamePadState lastPadState;

        /// <summary>
        /// A private constructor for the input manager that can only
        /// be called from the Initialise method.
        /// </summary>
        private InputManager()
        {
            inputManager = this;
        }

        /// <summary>
        /// Initialise the input manager.
        /// </summary>
        /// <returns>The input manager.</returns>
        public static InputManager Initialise()
        {
            if (inputManager == null)
            {
                new InputManager();
            }

            return inputManager;
        }

        /// <summary>
        /// Update the input.
        /// </summary>
        public void Update()
        {
            // Set the last states to the current states (as they
            // were last set on the previous frame)
            lastKeyState = keyState;
            lastPadState = padState;

            // Get the current state of the inputs
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Get whether the input binding is pressed.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the input binding is pressed, false if not.</returns>
        public static bool IsBindingPressed(InputBinding binding)
        {
            return IsKeyDown(binding) || IsButtonDown(binding);
        }

        /// <summary>
        /// Get whether the input binding is held down.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the input binding is held, false if not.</returns>
        public static bool IsBindingHeld(InputBinding binding)
        {
            return IsKeyHeld(binding) || IsButtonHeld(binding);
        }

        /// <summary>
        /// Get whether a keyboard button is pressed.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the keyboard button is down on this frame,
        /// but was not on the previous frame, false if not.</returns>
        private static bool IsKeyDown(InputBinding binding)
        {
            return (inputManager.keyState.IsKeyDown(binding.Key)
                || inputManager.keyState.IsKeyDown(binding.AltKey))
                && (inputManager.lastKeyState.IsKeyDown(binding.Key) == false
                && inputManager.lastKeyState.IsKeyDown(binding.AltKey) == false);
        }

        /// <summary>
        /// Get whether a keyboard button is held down.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the keyboard button is down on this frame,
        /// and the previous frame, false if not.</returns>
        private static bool IsKeyHeld(InputBinding binding)
        {
            return (inputManager.keyState.IsKeyDown(binding.Key)
                || inputManager.keyState.IsKeyDown(binding.AltKey))
                && (inputManager.lastKeyState.IsKeyDown(binding.Key)
                || inputManager.lastKeyState.IsKeyDown(binding.AltKey));
        }

        /// <summary>
        /// Get whether a gamepad button is pressed.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the gamepad button is down on this frame,
        /// but was not on the previous frame, false if not.</returns>
        private static bool IsButtonDown(InputBinding binding)
        {
            return (inputManager.padState.IsButtonDown(binding.Button)
                || inputManager.padState.IsButtonDown(binding.AltButton))
                && (inputManager.lastPadState.IsButtonDown(binding.Button) == false
                && inputManager.lastPadState.IsButtonDown(binding.AltButton) == false);
        }

        /// <summary>
        /// Get whether a gamepad button is held down.
        /// </summary>
        /// <param name="binding">The input binding.</param>
        /// <returns>true if the gamepad button is down on this frame,
        /// and the previous frame, false if not.</returns>
        private static bool IsButtonHeld(InputBinding binding)
        {
            return (inputManager.padState.IsButtonDown(binding.Button)
                || inputManager.padState.IsButtonDown(binding.AltButton))
                && (inputManager.lastPadState.IsButtonDown(binding.Button)
                || inputManager.lastPadState.IsButtonDown(binding.AltButton));
        }
    }
}
