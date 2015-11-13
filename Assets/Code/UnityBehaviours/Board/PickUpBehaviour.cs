using Assets.Code.DataPipeline;
using Assets.Code.Extensions;
using UnityEngine;

namespace Assets.Code.UnityBehaviours.Board
{
    internal delegate void OnIsSelectedValueChangedEventHandler(bool isSelected);
    internal delegate void OnIsHoveredValueChangedEventHandler(bool isHovered);
    
    internal class PickUpBehaviour : InitializeRequiredBehaviour
    {
        /* CONSTANTS */
        private static readonly Vector3 HoveredUpVector = new Vector3(0, 3f, 0);
        private static readonly Vector3 SelectedUpVector = new Vector3(0, 1f, 0);
        private const float LerpTime = 1f;
        private const float LerpPow = 1f;

        /* REFERENCES */
        private UnityReferenceMaster _unity;

        /* PROPERTIES */
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;
                if (OnIsSelectedValueChangedEvent != null)
                    OnIsSelectedValueChangedEvent(value);
            }
        }
        public OnIsSelectedValueChangedEventHandler OnIsSelectedValueChangedEvent;

        private bool _isHovered;
        public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                if (_isHovered == value) return;

                _isHovered = value;
                if (OnIsHoveredValueChangedEvent != null)
                    OnIsHoveredValueChangedEvent(value);
            }
        }
        public OnIsHoveredValueChangedEventHandler OnIsHoveredValueChangedEvent;

        // lerpity loo
        public bool IsRising { get; private set; }
        public bool IsFalling { get; private set; }
        
        private Vector3 _targetFallPosition;
        private Vector3 _oldPosition;
        private float _lerpProgress;

        public void Initialize(IoCResolver resolver)
        {
            resolver.Resolve(out _unity);

            IsSelected = false;
            IsHovered = false;
            OnIsSelectedValueChangedEvent += OnIsSelectedChanged;
            OnIsHoveredValueChangedEvent += OnIsHoveredChanged;

            MarkAsInitialized();
        }

        public void Update()
        {
            if(IsRising)
                HandleRiseLerp();
            else if(IsFalling)
                HandleFallLerp();
            else if (IsSelected)
                transform.position = _unity.MouseXzPlanePosition + SelectedUpVector;

            // if unselected and the mouse is over our board space, let us raise to the sky
            if (!IsSelected && _unity.MouseXzPlanePosition == transform.position.ToBoardVector(1, 0, 1))
                IsHovered = true;

            // drop the pick up if the mouse is released while we are selected
            if (Input.GetMouseButtonUp(0) && IsSelected)
            {
                _targetFallPosition = _unity.MouseXzPlanePosition.ToBoardVector(1, 0, 1);

                // will trigger all the falling stuff
                IsSelected = false;
            }
        }

        public void OnMouseDown()
        {
            IsSelected = true;
        }

        private void OnIsSelectedChanged(bool isSelected)
        {
            if (isSelected) StartRiseLerp();
            else StartFallLerp();
        }

        private void OnIsHoveredChanged(bool isHovered)
        {
            if (isHovered) StartRiseLerp();
            else StartFallLerp();
        }

        #region LERPITY LOO
        private void StartRiseLerp()
        {
            _lerpProgress = 0f;
            _oldPosition = transform.position;
            IsRising = true;
            IsFalling = false;
        }

        private void HandleRiseLerp()
        {
            _lerpProgress += Time.deltaTime;
            var lerpPowdPercent = Mathf.Pow(_lerpProgress / LerpTime, LerpPow);

            transform.position = Vector3.Lerp(_oldPosition, _unity.MouseXzPlanePosition + 
                                              (IsSelected ? SelectedUpVector : HoveredUpVector),
                                              lerpPowdPercent);

            if(_lerpProgress >= LerpTime)
                EndRiseLerp();
        }

        private void EndRiseLerp()
        {
            IsRising = false;
        }

        private void StartFallLerp()
        {
            _lerpProgress = 0f;
            _oldPosition = transform.position;
            IsFalling = true;
            IsRising = false;
        }

        private void HandleFallLerp()
        {
            _lerpProgress += Time.deltaTime;
            var lerpPowdPercent = Mathf.Pow(_lerpProgress/LerpTime, LerpPow);

            transform.position = Vector3.Lerp(_oldPosition, _targetFallPosition, lerpPowdPercent);

            if (_lerpProgress >= LerpTime)
                EndFallLerp();
        }

        private void EndFallLerp()
        {
            IsFalling = false;
        }
        #endregion
    }
}
