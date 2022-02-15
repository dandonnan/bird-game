namespace BirdGame.UI
{
    using BirdGame.Audio;
    using BirdGame.Data;
    using BirdGame.Events;
    using BirdGame.Input;
    using BirdGame.Text;
    using BirdGame.World;
    using System.Collections.Generic;

    internal class PauseScreen
    {
        private List<MenuOption> options;

        private int currentOption;

        public PauseScreen()
        {
            Reset();
        }

        public void Reset()
        {
            PopulateOptions();
        }
        
        public void Update()
        {
            options[currentOption].Update();

            if (InputManager.IsBindingPressed(DefaultBindings.Up))
            {
                currentOption--;

                if (currentOption < 0)
                {
                    currentOption = options.Count - 1;
                }
            }

            if (InputManager.IsBindingPressed(DefaultBindings.Down))
            {
                currentOption++;

                if (currentOption >= options.Count)
                {
                    currentOption = 0;
                }
            }

            if (currentOption == options.Count - 1 && 
                InputManager.IsBindingPressed(DefaultBindings.Dive))
            {
                SaveManager.Save();
                MainGame.QuitGame();
            }
        }

        public void Draw()
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (currentOption == i)
                {
                    options[i].DrawHighlighted();
                }
                else
                {
                    options[i].Draw();
                }
            }
        }

        private void PopulateOptions()
        {
            currentOption = 0;

            options = new List<MenuOption>
            {
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
                new MenuOption(
                    StringLibrary.GetString("Quit"),
                    null,
                    0,
                    180,
                    null),
            };
        }

        private void ChangeVolume(int volume)
        {
            if (volume < SaveData.MinVolume)
            {
                volume = SaveData.MinVolume;
            }
            else if (volume > SaveData.MaxVolume)
            {
                volume = SaveData.MaxVolume;
            }

            SaveManager.SaveData.SoundEffectsVolume = volume;

            AudioManager.ChangeVolume();
        }

        private void ChangeResolution()
        {
            WorldManager.GraphicsDeviceManager.PreferredBackBufferWidth = SaveManager.SaveData.ResolutionWidth;
            WorldManager.GraphicsDeviceManager.PreferredBackBufferHeight = SaveManager.SaveData.ResolutionHeight;

            WorldManager.GraphicsDeviceManager.ApplyChanges();

            EventManager.FireEvent(KnownEvents.ResolutionChanged);
        }

        private void ChangeFullscreen()
        {
            if (SaveManager.SaveData.Fullscreen)
            {
                WorldManager.GraphicsDeviceManager.PreferredBackBufferWidth = WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Width;
                WorldManager.GraphicsDeviceManager.PreferredBackBufferHeight = WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Height;

                EventManager.FireEvent(KnownEvents.ResolutionChanged);
            }
            else
            {
                ChangeResolution();
            }

            WorldManager.GraphicsDeviceManager.IsFullScreen = SaveManager.SaveData.Fullscreen;
            WorldManager.GraphicsDeviceManager.ApplyChanges();
        }

        private static readonly List<string> resolutions = new List<string>
        {
            "320x240", "640x480", "1280x720", "1920x1080"
        };

        private static readonly List<string> fullscreenOptions = new List<string>
        {
            "No", "Yes"
        };
    }
}
