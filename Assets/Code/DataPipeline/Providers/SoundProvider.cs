using System.Collections.Generic;
using Assets.Code.Logic.Logging;
using UnityEngine;

namespace Assets.Code.DataPipeline.Providers
{
    class SoundProvider : IResolvableItem
    {
        /* PROPERTIES */
        private readonly Logger _logger;
        private readonly Dictionary<string, AudioClip> _sounds;

        public SoundProvider(Logger logger)
        {
            _logger = logger;
            _sounds = new Dictionary<string, AudioClip>();
        }

        public void AddSound(AudioClip sound)
        {
            if(!_sounds.ContainsKey(sound.name))
                _sounds.Add(sound.name, sound);
        }

        public AudioClip GetSound(string query)
        {
            if (_sounds.ContainsKey(query))
                return _sounds[query];

            _logger.Log("WARNING! sound " + query + " does not exist!", true);
            return null;
        }
    }
}
