using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace AWP
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Animator))]
    [DefaultExecutionOrder(AWExecutionOrder.Camera2D)]
    public class AWCamera : MonoBehaviour
    {   
        public const AWDelta.DeltaType DefaultDeltaType = AWDelta.DeltaType.Update;

        [SerializeField]
        private DimensionType _positionType;
        [SerializeField]
        private List<Camera> _syncedCameras;
        [SerializeField]
        private TransparencySortMode _transparencySortMode = TransparencySortMode.Default;

        private Camera _camera;
        private Animator _animator;
        private Coroutine _moveRoutine;
        private Coroutine _sizeRoutine;
        private Coroutine _rotationRoutine;
        private Coroutine _animationRoutine;

        public Camera Camera => _camera;
        public Animator Animator => _animator;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _camera.transparencySortMode = _transparencySortMode;
            _animator.enabled = false;
        }

        public void MoveToCamPos(CameraPos camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = DefaultDeltaType, Action onFinish = null)
        {
            StartMoveRoutine(MoveToPositionRoutine(() => camPos.transform.position, duration, easing, deltaType, onFinish));
            StartSizeRoutine(MoveToSizeRoutine(() => camPos.OrthographicSize, duration, easing, deltaType));
            StartRotationRoutine(MoveToRotationRoutine(() => camPos.transform.rotation, duration, easing, deltaType));
        }
        public void MoveToCamPosSin(CameraPos camPos, float duration, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            MoveToCamPos(camPos, duration, EasingFunction.Sin, deltaType);
        }
        
        public void StartMoveRoutine(IEnumerator routine)
        {
            StopMoveRoutine();
            _moveRoutine = StartCoroutine(routine);
        }
        private void StopMoveRoutine() 
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
        }
        private IEnumerator MoveToPositionRoutine(Func<Vector3> func, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = DefaultDeltaType, Action onFinish = null)
        {
            Vector3 startPos = transform.position;
            
            yield return AnimationFX.DeltaRoutine((delta) =>
            {
                SetPosition(startPos + (func() - startPos) * delta);
            }, duration, easing, deltaType);

            onFinish?.Invoke();
            StartMoveRoutine(HoldPositionRoutine(func, deltaType));
        }
        private IEnumerator HoldPositionRoutine(Func<Vector3> func, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            while (true)
            {
                try 
                {
                    SetPosition(func());
                }
                catch
                {
                    yield break;
                }

                yield return deltaType.YieldNull();
            }
        }

        public void StartSizeRoutine(IEnumerator routine)
        {
            StopSizeRoutine();
            _sizeRoutine = StartCoroutine(routine);
        }
        private void StopSizeRoutine() 
        {
            if (_sizeRoutine != null) StopCoroutine(_sizeRoutine);
        }
        private IEnumerator MoveToSizeRoutine(Func<float> func, float duration, EasingFunction easing, AWDelta.DeltaType deltaType, Action onFinish = null)
        {
            float startSize = _camera.orthographicSize;

            yield return AnimationFX.DeltaRoutine((delta) =>
            {
                if (func == null) 
                {
                    StopSizeRoutine();
                    return;
                }

                SetSize(startSize + (func() - startSize) * delta);
            }, duration, easing, deltaType);

            onFinish?.Invoke();
            StartSizeRoutine(HoldSizeRoutine(func, deltaType));
        }
        private IEnumerator HoldSizeRoutine(Func<float> func, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            while (true)
            {
                if (func == null) yield break;
                SetSize(func());
                yield return deltaType.YieldNull();
            }
        }

        public void StartRotationRoutine(IEnumerator routine)
        {
            StopRotationRoutine();
            _rotationRoutine = StartCoroutine(routine);
        }
        public void StopRotationRoutine() 
        {
            if (_rotationRoutine != null) StopCoroutine(_rotationRoutine);
        }
        private IEnumerator MoveToRotationRoutine(Func<Quaternion> func, float duration, EasingFunction easing, AWDelta.DeltaType deltaType, Action onFinish = null)
        {
            Quaternion startRotation = _camera.transform.rotation;

            yield return AnimationFX.DeltaRoutine((delta) => 
            {
                SetRotation(Quaternion.Lerp(startRotation, func(), delta));
            }, duration, easing, deltaType);

            onFinish?.Invoke();
            StartRotationRoutine(HoldRotationRoutine(func, deltaType));
        }
        private IEnumerator HoldRotationRoutine(Func<Quaternion> func, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            while (true)
            {
                try { SetRotation(func()); }
                catch { yield break; }

                yield return deltaType.YieldNull();
            }
        }

        public void StopAllShiftRoutines()
        {
            StopMoveRoutine();
            StopSizeRoutine();
            StopRotationRoutine();
            StopAnimationRoutine();
        }

        private void SetPosition(Vector3 position)
        {
            switch (_positionType)
            {
                case DimensionType.XY:
                    transform.position = transform.position.SetXY(position);
                    break;
                case DimensionType.XYZ:
                    transform.position = transform.position = position;
                    break;
            }
        }
        private void SetSize(float size)
        {
            ModifyCamera((x) => x.orthographicSize = size);
        }
        private void SetRotation(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }

        #region Animation override
            public void StartAnimationRoutine(IEnumerator routine)
            {
                StopAllShiftRoutines();
                _animator.enabled = true;
                _animationRoutine = StartCoroutine(routine);
            }
            public void StopAnimationRoutine()
            {
                if (_animationRoutine != null) StopCoroutine(_animationRoutine);
                _animator.enabled = false;
            }
            public Coroutine OneShotPlayAnimation(string animation)
            {
                StartAnimationRoutine(OneShotPlay());
                return _animationRoutine;

                IEnumerator OneShotPlay()
                {
                    _animator.Play(animation);
                    yield return _animator.WaitForAnimationToComplete();
                    StopAnimationRoutine();
                }
            }
        #endregion

        private void ModifyCamera(Action<Camera> action)
        {
            action(_camera);
            _syncedCameras.ForEach((x) => action(x));
        }
    }
}
