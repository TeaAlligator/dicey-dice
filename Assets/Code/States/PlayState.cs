﻿using System;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages.Play;
using Assets.Code.Ui;
using Assets.Code.Ui.CanvasControllers;
using Assets.Code.UnityBehaviours.Board;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

            var tokenFab = Object.Instantiate(_prefabProvider.GetPrefab("pog_prefab")).GetComponent<PogController>();
            tokenFab.Initialize("https://upload.wikimedia.org/wikipedia/commons/b/b0/PSM_V37_D105_English_tabby_cat.jpg");
            tokenFab.PickUp.Initialize(_resolver);
            tokenFab.name = "token";
            tokenFab.transform.position = new Vector3(0, 1, 0);
        }

        public override void Update() {}

        public override void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }

        // http://answers.unity3d.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html
        private bool WasJustADamnedButton()
        {
            var ct = EventSystem.current;

            if (!ct.IsPointerOverGameObject()) return false;
            if (!ct.currentSelectedGameObject) return false;
            if (ct.currentSelectedGameObject.GetComponent<Button>() == null)
                return false;

            return true;
        }

        public override void TearDown()
        {
            _messager.CancelSubscription(_onRollDiceClicked, _onExitPlayClicked);
        }
    }
}
