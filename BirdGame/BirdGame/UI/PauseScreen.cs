namespace BirdGame.UI
{
    using BirdGame.Audio;
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Graphics;
    using BirdGame.Input;
    using BirdGame.Text;
    using BirdGame.World;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// The pause screen.
    /// </summary>
    internal class PauseScreen
    {
        /// <summary>
        /// The background of the screen.
        /// </summary>
        private readonly Sprite background;

        /// <summary>
        /// A list of options.
        /// </summary>
        private List<MenuOption> options;

        /// <summary>
        /// The index of the current option.
        /// </summary>
        private int currentOption;

        /// <summary>
        /// The constructor of the pause screen.
        /// </summary>
        public PauseScreen()
        {
            // Get the background
            background = SpriteLibrary.GetSprite("UiBackground");

            // Set the background to be off screen
            background.SetPosition(new Vector2(-32, -32));

            // Increase the scale so it covers the entire screen
            background.SetScale(25);

            Reset();
        }

        /// <summary>
        /// Reset the pause screen.
        /// </summary>
        public void Reset()
        {
            PopulateOptions();
        }
        
        /// <summary>
        /// Update the pause screen.
        /// </summary>
        public void Update()
        {
            // Update the current option
            options[currentOption].Update();

            // If the up button is pressed
            if (InputManager.IsBindingPressed(DefaultBindings.Up))
            {
                // Move to the previous option
                currentOption--;

                // If the current option is less than 0 (the first option)
                if (currentOption < 0)
                {
                    // Set the current option to wrap to the last option
                    currentOption = options.Count - 1;
                }
            }

            // If the down button is pressed
            if (InputManager.IsBindingPressed(DefaultBindings.Down))
            {
                // Move to the next option
                currentOption++;

                // If the current option is greater than the number of options
                if (currentOption >= options.Count)
                {
                    // Set the current option to wrap to the first option
                    currentOption = 0;
                }
            }

            // If the current option is the last one and the dive button
            // is pressed
            if (currentOption == options.Count - 1 && 
                InputManager.IsBindingPressed(DefaultBindings.Dive))
            {
                // Save the options
                SaveManager.Save();

                // Close the game
                MainGame.QuitGame();
            }
        }

        /// <summary>
        /// Draw the pause screen.
        /// </summary>
        public void Draw()
        {
            // Draw the background
            background.Draw();

            // Go through each of the options
            for (int i = 0; i < options.Count; i++)
            {
                // If the option is the current one
                if (currentOption == i)
                {
                    // Draw the option, but highlighted
                    options[i].DrawHighlighted();
                }
                // If the option is not the current one
                else
                {
                    // Draw the option
                    options[i].Draw();
                }
            }
        }

        /// <summary>
        /// Populate the options.
        /// </summary>
        private void PopulateOptions()
        {
            // Set the current option to the first one
            currentOption = 0;

            // Setup the list of options
            options = new List<MenuOption>
            {
                // Set the text of the option, the options, the current value, the y-offset
                // and the method when the option has changed
                new MenuOption(
                    StringLibrary.GetString("SoundEffects"), 
                    new List<string>
                    {
                        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"
                    },
                    SaveManager.SaveData.SoundEffectsVolume,
                    30,
                    (int option) =>
                    {
                        ChangeVolume(option);
                    }),
                new MenuOption(
                    StringLibrary.GetString("Resolution"),
                    resolutions,
                    resolutions.IndexOf(SaveManager.SaveData.Resolution),
                    80,
                    (int option) =>
                    {
                        SaveManager.SaveData.Resolution = resolutions[option];
                        ChangeResolution();
                    }),
                new MenuOption(
                    StringLibrary.GetString("Fullscreen"),
                    fullscreenOptions,
                    SaveManager.SaveData.Fullscreen ? 1 : 0,
                    130,
                    (int option) =>
                    {
                        SaveManager.SaveData.Fullscreen = option == 0 ? false : true;
                        ChangeFullscreen();
                    }),
                // The quit option has no option to choose from, and no method to call
                new MenuOption(
                    StringLibrary.GetString("Quit"),
                    null,
                    0,
                    180,
                    null),
            };
        }

        /// <summary>
        /// Change the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        private void ChangeVolume(int volume)
        {
            // If the volume is less than the minimum
            if (volume < SaveData.MinVolume)
            {
                // Set the volume to the minimum
                volume = SaveData.MinVolume;
            }
            // If the volume is greater than the maximum
            else if (volume > SaveData.MaxVolume)
            {
                // Set the volume to the maximum
                volume = SaveData.MaxVolume;
            }

            // Set the save data volume to the current volume
            SaveManager.SaveData.SoundEffectsVolume = volume;

            // Change the volume on the audio manager to update looping sounds
            AudioManager.ChangeVolume();
        }

        /// <summary>
        /// Change the resolution.
        /// </summary>
        private void ChangeResolution()
        {
            // If the game is not fullscreen
            if (SaveManager.SaveData.Fullscreen == false)
            {
                // Set the width and height based on the resolution
                WorldManager.GraphicsDeviceManager.PreferredBackBufferWidth = SaveManager.SaveData.ResolutionWidth;
                WorldManager.GraphicsDeviceManager.PreferredBackBufferHeight = SaveManager.SaveData.ResolutionHeight;

                // Apply the changes to the graphics device
                WorldManager.GraphicsDeviceManager.ApplyChanges();
            }

            // Fire the resolution changed event
            EventManager.FireEvent(KnownEvents.ResolutionChanged);
        }

        /// <summary>
        /// Change the game to be fullscreen.
        /// </summary>
        private void ChangeFullscreen()
        {
            // If the game is fullscreen
            if (SaveManager.SaveData.Fullscreen)
            {
                // Set the width and height based on the monitor's size
                WorldManager.GraphicsDeviceManager.PreferredBackBufferWidth = WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Width;
                WorldManager.GraphicsDeviceManager.PreferredBackBufferHeight = WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Height;

                // Fire the resolution changed event
                EventManager.FireEvent(KnownEvents.ResolutionChanged);
            }
            // If the game is not fullscreen
            else
            {
                // Change the resolution
                ChangeResolution();
            }

            // Set the game to be fullscreen based on the settings
            WorldManager.GraphicsDeviceManager.IsFullScreen = SaveManager.SaveData.Fullscreen;

            // Apply the changes to the graphics device
            WorldManager.GraphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// The list of options for resolutions.
        /// </summary>
        private static readonly List<string> resolutions = new List<string>
        {
            "640x480", "1280x720", "1600x900", "1920x1080"
        };

        /// <summary>
        /// The list of options for whether the game is fullscreen.
        /// </summary>
        private static readonly List<string> fullscreenOptions = new List<string>
        {
            "No", "Yes"
        };
    }
}
