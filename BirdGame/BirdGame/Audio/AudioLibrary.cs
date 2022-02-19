namespace BirdGame.Audio
{
    using BirdGame.World;
    using Microsoft.Xna.Framework.Audio;
    using System.Collections.Generic;

    /// <summary>
    /// A library for audio files.
    /// </summary>
    internal class AudioLibrary
    {
        /// <summary>
        /// A singleton for the audio library, so there is only ever one instance.
        /// </summary>
        private static AudioLibrary audioLibrary;

        /// <summary>
        /// A dictionary of all the game's sound effects.
        /// </summary>
        private readonly Dictionary<string, SoundEffect> soundEffects;

        /// <summary>
        /// A private constructor, so the library can only be created through
        /// the Initialise method.
        /// </summary>
        private AudioLibrary()
        {
            soundEffects = PopulateDictionary();

            audioLibrary = this;
        }

        /// <summary>
        /// Initialise the audio manager.
        /// </summary>
        public static void Initialise()
        {
            if (audioLibrary == null)
            {
                new AudioLibrary();
            }
        }

        /// <summary>
        /// Get a sound effect from the library.
        /// </summary>
        /// <param name="id">The id of the sound effect.</param>
        /// <returns>The sound effect, or null if not found.</returns>
        public static SoundEffect GetSoundEffect(string id)
        {
            audioLibrary.soundEffects.TryGetValue(id, out SoundEffect soundEffect);

            return soundEffect;
        }

        /// <summary>
        /// Populate the dictionary of sound effects.
        /// </summary>
        /// <returns>A dictionary of sound effects.</returns>
        private Dictionary<string, SoundEffect> PopulateDictionary()
        {
            return new Dictionary<string, SoundEffect>
            {
                // The id of sound effect for in-game use, load in the sound effect from the files
                { "Ambience", WorldManager.ContentManager.Load<SoundEffect>("Sounds//seaside_ambience") },
                { "WingsFlap1", WorldManager.ContentManager.Load<SoundEffect>("Sounds//wing_flap1") },
                { "WingsFlap2", WorldManager.ContentManager.Load<SoundEffect>("Sounds//wing_flap2") },
                { "Poop", WorldManager.ContentManager.Load<SoundEffect>("Sounds//poop") },
                { "Splat", WorldManager.ContentManager.Load<SoundEffect>("Sounds//splat") },
                { "Dive", WorldManager.ContentManager.Load<SoundEffect>("Sounds//diving") },
                { "DroneEnter", WorldManager.ContentManager.Load<SoundEffect>("Sounds//drone_enter") },
                { "DroneFly", WorldManager.ContentManager.Load<SoundEffect>("Sounds//drone_idle") },
                { "DroneLeave", WorldManager.ContentManager.Load<SoundEffect>("Sounds//drone_exit") }
            };
        }
    }
}
