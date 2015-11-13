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

        // mouse position
        private static Plane _xzPlane = new Plane(Vector3.up, Vector3.zero);

        private bool _isMouseXzPlanePositionCached;
        private Vector3 _cachedMouseXzPlanePosition;
        public Vector3 MouseXzPlanePosition
        {
            get
            {
                if (!_isMouseXzPlanePositionCached)
                {
                    _cachedMouseXzPlanePosition = CalculateMouseXZPlanePosition();
                    _isMouseXzPlanePositionCached = true;
                }

                return _cachedMouseXzPlanePosition;
            }
        }

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
            _isMouseXzPlanePositionCached = false;
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
        
        public void Update()
        {
            // update mouse plane position
            _isMouseXzPlanePositionCached = false;

            // update sin
            CurrentStaticSinValue = Math.Sin(Time.timeSinceLevelLoad);

            // update delayed actions
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

        private Vector3 CalculateMouseXZPlanePosition()
        {
            float distance;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_xzPlane.Raycast(ray, out distance))
            {
                var hit = ray.GetPoint(distance);
                hit.y = 0f;

                return hit;
            }

            // really id like to return a bool or something
            // to signify if the trace was successful
            // but with the isometric camera i cant think of a way
            // to click on something other than the xz plane

            // so i dont really care
            return Vector3.zero;
        }
    }
}
