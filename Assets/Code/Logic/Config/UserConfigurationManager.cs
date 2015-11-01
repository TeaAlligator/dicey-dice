using Assets.Code.DataPipeline;
using Assets.Code.Models.Config;

namespace Assets.Code.Logic.Config
{
    delegate void OnConfigValueChangedEventHandler(float oldValue, float newValue);

    class UserConfigurationManager : IResolvableItem
    {
        private readonly UserConfigurationData _data;

        public float GameVolume {
            get { return _data.GameVolume; }
            set {
                var oldValue = _data.GameVolume;
                _data.GameVolume = value;

                if (OnGameVolumeChangedEvent != null)
                    OnGameVolumeChangedEvent(oldValue, value);
            }
        }
        public OnConfigValueChangedEventHandler OnGameVolumeChangedEvent;

        public float MusicVolume
        {
            get { return _data.MusicVolume; }
            set
            {
                var oldValue = _data.MusicVolume;
                _data.MusicVolume = value;

                if (OnMusicVolumeChangedEvent != null)
                    OnMusicVolumeChangedEvent(oldValue, value);
            }
        }
        public OnConfigValueChangedEventHandler OnMusicVolumeChangedEvent;

        public UserConfigurationManager(UserConfigurationData data){
            _data = data;
        }
    }
}
