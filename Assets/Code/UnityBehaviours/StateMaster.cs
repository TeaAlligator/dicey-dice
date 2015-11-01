using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Loading;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Logic.Config;
using Assets.Code.Logic.Logging;
using Assets.Code.Logic.Pooling;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.Models.Config;
using Assets.Code.States;
using Assets.Code.Ui;
using Assets.Code.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Code.UnityBehaviours
{
    [RequireComponent(typeof(UnityReferenceMaster))]
    class StateMaster : MonoBehaviour
    {
        /* REFERENCES */
        private UnityReferenceMaster _unityReference;

        /* PROPERTIES */
        private IoCResolver _resolver;
        private Logger _logger;
        private Messager _messager;
        private UserConfigurationManager _config;
        private PoolingParticleManager _particles;
        private UiManager _uiManager;

        private BaseState _currentState;

        /* TOKENS */
        private MessagingToken _onExit;

        public void Start()
        {
            /* RESOURCE LIST CREATION */
#if UNITY_EDITOR
            AssetDatabase.Refresh();
            FileServices.CreateResourcesList("Assets/Resources/resourceslist.txt");
#endif
            FileServices.LoadResourcesList("resourceslist");

            #region LOAD RESOURCES
            // logger and ioc
            _logger = new Logger("info.log", false);
            _resolver = new IoCResolver(_logger);
            _resolver.RegisterItem(_logger);
            
            _config = new UserConfigurationManager(new UserConfigurationData
            {
                GameVolume = 1f,
                MusicVolume = 1f
            });
            _resolver.RegisterItem(_config);

            // messager
            _messager = new Messager();
            _resolver.RegisterItem(_messager);

            // unity reference master
            _unityReference = GetComponent<UnityReferenceMaster>();
            _unityReference.DebugModeActive = false;
            _resolver.RegisterItem(_unityReference);

            // material provider
            var materialProvider = new MaterialProvider(_logger);
			MaterialLoader.LoadMaterial(materialProvider,"Materials");
            _resolver.RegisterItem(materialProvider);

            // texture provider
            var textureProvider = new TextureProvider(_logger);
            var spriteProvider = new SpriteProvider(_logger);
            TextureLoader.LoadTextures(textureProvider, spriteProvider, "Textures");
            _resolver.RegisterItem(textureProvider);
            _resolver.RegisterItem(spriteProvider);

            // sound provider
            var soundProvider = new SoundProvider(_logger);
            SoundLoader.LoadSounds(_logger, soundProvider, "Sounds");
            _resolver.RegisterItem(soundProvider);

            // prefab provider
            var prefabProvider = new PrefabProvider(_logger);
            PrefabLoader.LoadPrefabs(prefabProvider);
            _resolver.RegisterItem(prefabProvider);

            // pooling
            var poolingObjectManager = new PoolingObjectManager(_logger, prefabProvider);
            _resolver.RegisterItem(poolingObjectManager);

            var soundPoolManager = new PoolingAudioPlayer(_logger, _config, _unityReference, prefabProvider.GetPrefab("audio_source_prefab"));
            _resolver.RegisterItem(soundPoolManager);

            _particles = new PoolingParticleManager(_resolver);
            _resolver.RegisterItem(_particles);

            // canvas provider
            var canvasProvider = new CanvasProvider(_logger);
            _unityReference.LoadCanvases(canvasProvider);
            _resolver.RegisterItem(canvasProvider);

            // data provider
            var gameDataProvider = new GameDataProvider(_logger);

            /* DATA GOES HERE */

            _resolver.RegisterItem(gameDataProvider);
            #endregion

            _uiManager = new UiManager();
            _resolver.RegisterItem(_uiManager);

            // lock the resolver (stop any new items being registered)
            _resolver.Lock();

            /* BEGIN STATE */
            _currentState = new MenuState(_resolver);
            _currentState.Initialize();

            /* SUBSCRIBE FOR GAME END */
            _onExit = _messager.Subscribe<ExitMessage>(OnExit);
        }

        private void OnExit(ExitMessage message)
        {
            _logger.Log("game exited");

            _logger.Flush();

            Application.Quit();
        }

        public void OnApplicationQuit()
        {
            // handle thing
        }

        public void Update()
        {
            if (_currentState == null) return;

            /* SWITCH STATE IF NEEDED */
            if (_currentState.IsReadyForStateSwitch)
            {
                var previousState = _currentState;
                _currentState = _currentState.TargetSwitchState;
                
                _uiManager.ClearUi();
                previousState.TearDown();
                _unityReference.ResetDelayedActions();
                _currentState.Initialize();
            }

            /* UPDATE STATE */
            _currentState.Update ();
            _currentState.HandleInput();
        }

        public void FixedUpdate()
        {
            if(_particles != null)
                _particles.FixedUpdate();
        }
    }
}