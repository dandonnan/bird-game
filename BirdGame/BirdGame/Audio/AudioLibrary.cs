namespace BirdGame.Audio
{
    using BirdGame.World;
    using Microsoft.Xna.Framework.Audio;
    using System.Collections.Generic;

    internal class AudioLibrary
    {
        private static AudioLibrary audioLibrary;

        private readonly Dictionary<string, SoundEffect> soundEffects;

        private AudioLibrary()
        {
            soundEffects = PopulateDictionary();

            audioLibrary = this;
        }

        public static void Initialise()
        {
            if (audioLibrary == null)
            {
                new AudioLibrary();
            }
        }

        public static SoundEffect GetSoundEffect(string id)
        {
            audioLibrary.soundEffects.TryGetValue(id, out SoundEffect soundEffect);

            return soundEffect;
        }

        private Dictionary<string, SoundEffect> PopulateDictionary()
        {
            return new Dictionary<string, SoundEffect>
            {
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
