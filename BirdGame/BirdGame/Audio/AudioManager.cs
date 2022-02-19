namespace BirdGame.Audio
{
    using BirdGame.Data;
    using Microsoft.Xna.Framework.Audio;
    using System.Collections.Generic;

    /// <summary>
    /// A manager for audio.
    /// </summary>
    internal class AudioManager
    {
        /// <summary>
        /// A singleton for the audio manager, so there is only ever one.
        /// </summary>
        private static AudioManager audioManager;

        /// <summary>
        /// A dictionary of sound effect instances for sounds that loop.
        /// </summary>
        private readonly Dictionary<string, SoundEffectInstance> loopingSounds;

        /// <summary>
        /// A private constructor for the audio manager, so it can only be
        /// created through the Initialise method.
        /// </summary>
        private AudioManager()
        {
            loopingSounds = new Dictionary<string, SoundEffectInstance>();

            audioManager = this;
        }

        /// <summary>
        /// Initialise the audio manager.
        /// </summary>
        public static void Initialise()
        {
            if (audioManager == null)
            {
                new AudioManager();
            }
        }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        /// <param name="id">The id of the sound effect.</param>
        public static void PlaySoundEffect(string id)
        {
            // Get the sound effect from the library
            SoundEffect soundEffect = AudioLibrary.GetSoundEffect(id);

            if (soundEffect != null)
            {
                // If there is a sound effect, play it at the volume
                // from the settings, and with no pitch or pan
                soundEffect.Play(GetSoundVolume(), 0, 0);
            }
        }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        /// <param name="soundEffect">The sound effect.</param>
        public static void PlaySoundEffect(SoundEffect soundEffect)
        {
            // Play the sound effect at the volume from the settings,
            // and with no pitch or pan
            soundEffect.Play(GetSoundVolume(), 0, 0);
        }

        /// <summary>
        /// Play a sound effect that loops.
        /// </summary>
        /// <param name="id">The id of the sound effect.</param>
        public static void PlayLoopingSoundEffect(string id)
        {
            // Get the sound effect from the library
            SoundEffect soundEffect = AudioLibrary.GetSoundEffect(id);

            // If there is sound effect with the id, and it isn't already looping
            if (soundEffect != null && audioManager.loopingSounds.ContainsKey(id) == false)
            {
                // Create a sound effect instance from the sound effect
                SoundEffectInstance instance = soundEffect.CreateInstance();

                // Make the instance loop
                instance.IsLooped = true;

                // Set the volume based on the settings
                instance.Volume = GetSoundVolume();

                // Play the instance
                instance.Play();

                // Add the instance to the looping sounds dictionary, to prevent
                // if from triggering twice
                audioManager.loopingSounds.Add(id, instance);
            }
        }

        /// <summary>
        /// Get whether a looping sound is playing.
        /// </summary>
        /// <param name="id">The id of the sound.</param>
        /// <returns>true if the looping sound is playing, false if not.</returns>
        public static bool IsLoopingSoundPlaying(string id)
        {
            return audioManager.loopingSounds.ContainsKey(id);
        }

        /// <summary>
        /// Stop a sound effect from looping.
        /// </summary>
        /// <param name="id">The id of the sound effect.</param>
        public static void StopLoopingSoundEffect(string id)
        {
            // Get a sound effect instance from the dictionary
            audioManager.loopingSounds.TryGetValue(id, out SoundEffectInstance instance);

            if (instance != null)
            {
                // If there is a sound effect instance, then stop it
                instance.Stop();

                // Remove the sound so it can be started again later
                audioManager.loopingSounds.Remove(id);
            }
        }

        /// <summary>
        /// Change the volume of the sounds.
        /// </summary>
        public static void ChangeVolume()
        {
            // For every current looping sound, set the volume based on the volume in
            // the settings
            foreach (SoundEffectInstance sound in audioManager.loopingSounds.Values)
            {
                sound.Volume = GetSoundVolume();
            }
        }

        /// <summary>
        /// Get the volume for sound effects from the settings.
        /// </summary>
        /// <returns>The sound effect volume.</returns>
        private static float GetSoundVolume()
        {
            // The volume in the settings
            int volume = SaveManager.SaveData.SoundEffectsVolume;

            // Divide by 10 as volume is between 0 and 1, not 0 and 10.
            return (float)volume / 10;
        }
    }
}
