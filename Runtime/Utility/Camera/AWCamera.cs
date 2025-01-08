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
    [DefaultExecutionOrder(AWExecutionOrder.Camera2D)]
    public class AWCamera : MonoBehaviour
    {   
        public const AWDelta.DeltaType DefaultDeltaType = AWDelta.DeltaType.Update;

        [SerializeField]
        private PositionType _positionType;
        [SerializeField]
        private List<Camera> _syncedCameras;

        private Camera _camera;
        private Coroutine _moveRoutine;
        private Coroutine _sizeRoutine;
        private Coroutine _rotationRoutine;

        public enum PositionType { XY, XYZ }

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();
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
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(routine);
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
                SetPosition(func());
                yield return deltaType.YieldNull();
            }
        }

        public void StartSizeRoutine(IEnumerator routine)
        {
            if (_sizeRoutine != null) StopCoroutine(_sizeRoutine);
            _sizeRoutine = StartCoroutine(routine);
        }
        private IEnumerator MoveToSizeRoutine(Func<float> func, float duration, EasingFunction easing, AWDelta.DeltaType deltaType, Action onFinish = null)
        {
            float startSize = _camera.orthographicSize;

            yield return AnimationFX.DeltaRoutine((delta) =>
            {
                SetSize(startSize + (func() - startSize) * delta);
            }, duration, easing, deltaType);

            onFinish?.Invoke();
            StartSizeRoutine(HoldSizeRoutine(func, deltaType));
        }
        private IEnumerator HoldSizeRoutine(Func<float> func, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            while (true)
            {
                SetSize(func());
                yield return deltaType.YieldNull();
            }
        }

        public void StartRotationRoutine(IEnumerator routine)
        {
            if (_rotationRoutine != null) StopCoroutine(_rotationRoutine);
            _rotationRoutine = StartCoroutine(routine);
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
                SetRotation(func());
                yield return deltaType.YieldNull();
            }
        }

        private void SetPosition(Vector3 position)
        {
            switch (_positionType)
            {
                case PositionType.XY:
                    transform.position = transform.position.SetXY(position);
                    break;
                case PositionType.XYZ:
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

        private void ModifyCamera(Action<Camera> action)
        {
            action(_camera);
            _syncedCameras.ForEach((x) => action(x));
        }
    }
}
