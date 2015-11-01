using Assets.Code.Logic.Config;
using Assets.Code.Models.Pooling;
using UnityEngine;

namespace Assets.Code.UnityBehaviours.Pooling
{
    [RequireComponent(typeof(AudioSource))]
    class PooledAudioSource : InitializeRequiredBehaviour
    {
        /* REFERENCES */
        private UserConfigurationManager _config;
        private UnityReferenceMaster _unityReferenceMaster;
        private Transform _cameraTransform;
        private AudioSource _audio;

        /* PROPERTIES */
        private PooledAudioRequest _currentRequest;
        public bool IsActive { get { return _currentRequest != null; } }

        public void Initialize(UserConfigurationManager config, UnityReferenceMaster unityReferenceMaster)
        {
            _cameraTransform = unityReferenceMaster.Camera.transform;
            _audio = GetComponent<AudioSource>();

            _config = config;
            _unityReferenceMaster = unityReferenceMaster;
            _unityReferenceMaster.OnGamePausedChangedEvent += OnGamePausedChanged;
            _config.OnGameVolumeChangedEvent += OnGameVolumeChanged;
            _config.OnMusicVolumeChangedEvent += OnMusicVolumeChanged;

            MarkAsInitialized();
        }

        private void OnGamePausedChanged(bool isGamePaused)
        {
            if(IsActive && _audio.loop)
                if (isGamePaused)
                    _audio.Pause();
                else
                    _audio.Play();
        }

        private void OnGameVolumeChanged(float oldValue, float newValue)
        {
            if (_currentRequest == null || _currentRequest.IsMusic) return;

            _audio.volume = _currentRequest.Volume * newValue;
        }

        private void OnMusicVolumeChanged(float oldValue, float newValue)
        {
            if (_currentRequest == null || !_currentRequest.IsMusic) return;

            _audio.volume = _currentRequest.Volume * newValue;
        }

        public void PlaySound(PooledAudioRequest request)
        {
            _currentRequest = request;

            // bail if the sound is null
            if (request == null)
                return;

            _audio.loop = false;
            _audio.transform.position = request.IsSpatial ? request.Target : _cameraTransform.position;

            _audio.clip = request.Sound;
            _audio.volume = request.Volume * (request.IsMusic
                                ? _config.MusicVolume
                                : _config.GameVolume);
            _audio.Play();
        }

        public AudioLoopToken LoopSound(PooledAudioRequest request)
        {
            _currentRequest = request;

            // bail gracefully if request is null
            if (request == null)
                return new AudioLoopToken
                {
                    Replace = replacementRequest => LoopSound(replacementRequest),
                    End = () => { }
                };

            _audio.loop = true;
            _audio.transform.position = request.IsSpatial ? request.Target : _cameraTransform.position;

            _audio.clip = request.Sound;
            _audio.volume = request.Volume * (request.IsMusic
                                ? _config.MusicVolume
                                : _config.GameVolume);
            _audio.Play();

            return new AudioLoopToken
            {
                Replace = replacementRequest => LoopSound(replacementRequest),
                End = () => Stop()
            };
        }

        public void Stop()
        {
            _audio.Stop();
            _currentRequest = null;
        }

        public void FixedUpdate()
        {
            if (_currentRequest == null || _unityReferenceMaster.IsPaused) return;

            // once finished, we move on to the next sound
            // or it will be null (both are cool)
            if (!_audio.isPlaying)
            {
                if(_currentRequest.OnFinished != null)
                    _currentRequest.OnFinished(this);
                else
                    Stop();
            }

            else if (!_currentRequest.IsSpatial)
                transform.position = _cameraTransform.position;
        }

        public void Delete()
        {
            _unityReferenceMaster.OnGamePausedChangedEvent -= OnGamePausedChanged;
        }
    }
}
