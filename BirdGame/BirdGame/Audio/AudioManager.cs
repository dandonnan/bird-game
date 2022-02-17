namespace BirdGame.Audio
{
    using BirdGame.Data;
    using Microsoft.Xna.Framework.Audio;
    using System.Collections.Generic;

    internal class AudioManager
    {
        private static AudioManager audioManager;

        private Dictionary<string, SoundEffectInstance> loopingSounds;

        private AudioManager()
        {
            loopingSounds = new Dictionary<string, SoundEffectInstance>();

            audioManager = this;
        }

        public static void Initialise()
        {
            if (audioManager == null)
            {
                new AudioManager();
            }
        }

        public static void PlaySoundEffect(string id)
        {
            SoundEffect soundEffect = AudioLibrary.GetSoundEffect(id);

            if (soundEffect != null)
            {
                soundEffect.Play(GetSoundVolume(), 0, 0);
            }
        }

        public static void PlaySoundEffect(SoundEffect soundEffect)
        {
            soundEffect.Play(GetSoundVolume(), 0, 0);
        }

        public static void PlayLoopingSoundEffect(string id)
        {
            SoundEffect soundEffect = AudioLibrary.GetSoundEffect(id);

            if (soundEffect != null && audioManager.loopingSounds.ContainsKey(id) == false)
            {
                SoundEffectInstance instance = soundEffect.CreateInstance();
                instance.IsLooped = true;
                instance.Volume = GetSoundVolume();
                instance.Play();

                audioManager.loopingSounds.Add(id, instance);
            }
        }

        public static void StopLoopingSoundEffect(string id)
        {
            audioManager.loopingSounds.TryGetValue(id, out SoundEffectInstance instance);

            if (instance != null)
            {
                instance.Stop();
                audioManager.loopingSounds.Remove(id);
            }
        }

        public static void ChangeVolume()
        {
            foreach (SoundEffectInstance sound in audioManager.loopingSounds.Values)
            {
                sound.Volume = GetSoundVolume();
            }
        }

        private static float GetSoundVolume()
        {
            int volume = SaveManager.SaveData.SoundEffectsVolume;

            return (float)volume / 10;
        }
    }
}
