using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace AWP
{
    public abstract class UIFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected RectTransform _rect;
        protected bool _isHighlighted;
        private SingleCoroutine _updateRoutine;
        private SingleCoroutine _fixedUpdateRoutine;
        private Vector2 _mousePosOld;

        private int _routineCount;

        protected virtual float DeltaTime => Time.unscaledDeltaTime;
        protected virtual float FixedDeltaTime => Time.fixedUnscaledDeltaTime;
        protected Vector2 MousePos => new Vector2(Mouse.current.position.x.value, Mouse.current.position.y.value);
        protected Vector2 MouseWorldPos => AWGameManager.AWCamera.Camera.ScreenToWorldPoint(MousePos);
        protected Vector2 MouseScreenOffset => MousePos - (Vector2)_rect.position;
        protected Vector2 MouseDelta => new Vector2(-1 + 2 * MousePos.x.GetDelta(_rect.position.x - _rect.sizeDelta.x / 2,
            _rect.position.x + _rect.sizeDelta.x / 2), -1 + 2 * MousePos.y.GetDelta(_rect.position.y - _rect.sizeDelta.y / 2,
            _rect.position.y + _rect.sizeDelta.y / 2));
        protected Vector2 MouseVelocity { get; private set; }

        protected virtual void Awake()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable()
        {
            FXReset();
        }

        protected virtual void OnDisable()
        {
            StopAnimationRoutines();
        }

        protected virtual void Start()
        {
            _updateRoutine = new SingleCoroutine(this);
            _fixedUpdateRoutine = new SingleCoroutine(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _mousePosOld = MousePos;

            _isHighlighted = true;
            StartAnimationRoutines();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _isHighlighted = false;
        }

        protected void StartAnimationRoutines()
        {
            _fixedUpdateRoutine.StartRoutine(FixedUpdateRoutine());
            _updateRoutine.StartRoutine(UpdateRoutine());
        }

        protected void StopAnimationRoutines()
        {
            _fixedUpdateRoutine.StopRoutine();
            _updateRoutine.StopRoutine();
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                FXUpdate(DeltaTime);

                if (!_fixedUpdateRoutine.RoutineActive)
                {
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator FixedUpdateRoutine()
        {
            while (true)
            {
                UpdateVariables();
                FXFixedUpdate(FixedDeltaTime);
                SyncOldVariables();

                // End routine if finished
                if (FXFinished())
                {
                    FXReset();
                    yield break;
                }

                yield return AWDelta.YieldNull(AWDelta.DeltaType.UnscaledFixedUpdate);
            }
        }

        private void UpdateVariables()
        {
            MouseVelocity = MousePos - _mousePosOld;
        }

        private void SyncOldVariables()
        {
            _mousePosOld = MousePos;
        }

        protected abstract void FXReset();
        protected abstract void FXUpdate(float deltaTime);
        protected abstract void FXFixedUpdate(float deltaTime);
        protected abstract bool FXFinished();
    }
}
