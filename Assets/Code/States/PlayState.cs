using System;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages.Play;
using Assets.Code.Ui;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Code.States
{
    class PlayState : BaseState
    {
        /* REFERENCES */

        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
        private readonly PrefabProvider _prefabProvider;
        private readonly UiManager _uiManager;

        /* TOKENS */
        private MessagingToken _onRollDiceClicked;
        private MessagingToken _onExitPlayClicked;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            // resolve
            resolver.Resolve(out _messager);
            resolver.Resolve(out _canvasProvider);
            resolver.Resolve(out _prefabProvider);
            resolver.Resolve(out _uiManager);
        }

        public override void Initialize()
        {
            // initialize
            _uiManager.RegisterUi(new PlayCanvasController(_resolver, _canvasProvider.GetCanvas("play_canvas")));

            _onExitPlayClicked = _messager.Subscribe<ExitPlayClickedMessage>(message =>
            {
                SwitchState(new MenuState(_resolver));
            });

            _onRollDiceClicked = _messager.Subscribe<RollDiceClickedMessage>(message =>
            {
                var rollMagnitude = 250f;
                var rollDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

                for (var i = 0; i < 3; i++)
                {
                    var fab = Object.Instantiate(_prefabProvider.GetPrefab("dice_d6_prefab"));
                    fab.transform.name = "dice_" + Guid.NewGuid().ToString();
                    fab.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    fab.transform.position = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f));

                    var fabRigidBody = fab.GetComponent<Rigidbody>();
                    fabRigidBody.AddForce(rollDirection * rollMagnitude);
                    fabRigidBody.AddRelativeTorque(rollDirection * 10);
                }
            });
        }

        public override void Update() {}

        public override void HandleInput() {}

        public override void TearDown()
        {
            _messager.CancelSubscription(_onRollDiceClicked, _onExitPlayClicked);
        }
    }
}
