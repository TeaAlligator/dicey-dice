using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Models;
using UnityEngine;

namespace Assets.Code.UnityBehaviours
{
    public delegate void OnDebugModeChangedEventHandler(bool debugModeActive);
    public delegate void OnGamePausedChangedEventHandler(bool isPaused);

    class UnityReferenceMaster : MonoBehaviour, IResolvableItem
    {
        /* REFERENCES */
        public Camera Camera;
        private Transform _shakePerspectiveReference;

        /* PROPERTIES */
        private List<DelayedAction> _delayedActions;

        public static double CurrentStaticSinValue;
        public double CurrentSinValue { get { return CurrentStaticSinValue; } }

        private bool _debugModeActive;
        public bool DebugModeActive
        {
            get { return _debugModeActive; }
            set
            {
                _debugModeActive = value;

                if (OnDebugModeChangedEvent != null)
                    OnDebugModeChangedEvent(value);
            }
        }
        public OnDebugModeChangedEventHandler OnDebugModeChangedEvent;

        public bool IsPaused { get; private set; }
        public OnGamePausedChangedEventHandler OnGamePausedChangedEvent;

        public void Awake()
        {
            _delayedActions = new List<DelayedAction>();
        }

        public void PauseGame()
        {
            IsPaused = true;

            if(OnGamePausedChangedEvent != null)
                OnGamePausedChangedEvent(IsPaused);
        }
        public void ResumeGame()
        {
            IsPaused = false;

            if (OnGamePausedChangedEvent != null)
                OnGamePausedChangedEvent(IsPaused);
        }

        public void FireDelayed(Action action, float delayTime = 0f, bool ignorePaused = false)
        {
            _delayedActions.Add(new DelayedAction
            {
                Payload = action,
                RemainingTime = delayTime,
                IgnorePaused = ignorePaused
            });
        }

        public void VibrateDevice()
        {
            #if UNITY_ANDROID
                Handheld.Vibrate();
            #endif
        }

        public void RegisterShakePerspectiveTransform(Transform shakePerspective)
        {
            _shakePerspectiveReference = shakePerspective;
        }

        public void LoadCanvases(CanvasProvider canvasProvider)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var childCanvas = transform.GetChild(i).GetComponent<Canvas>();

                if (childCanvas != null)
                {
                    childCanvas.gameObject.SetActive(false);
                    canvasProvider.AddCanvas(childCanvas);
                }
            }
        }

        public void FixedUpdate()
        {
            CurrentStaticSinValue = Math.Sin(Time.timeSinceLevelLoad);

            for (var i = 0; i < _delayedActions.Count; i++)
            {
                if (IsPaused && !_delayedActions[i].IgnorePaused) return;
                _delayedActions[i].RemainingTime -= Time.deltaTime;

                if (_delayedActions[i].RemainingTime <= 0)
                {
                    _delayedActions[i].Payload();
                    _delayedActions.RemoveAt(i);

                    i--;
                }
            }
        }

        public void ResetDelayedActions()
        {
            if(_delayedActions != null)
                _delayedActions.Clear();
        }
    }
}
