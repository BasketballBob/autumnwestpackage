using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(AWExecutionOrder.Camera2D)]
    public class Camera2D : MonoBehaviour
    {   
        private const AWDelta.DeltaType DefaultDeltaType = AWDelta.DeltaType.Update;

        private Camera _camera;
        private Coroutine _moveRoutine;
        private Coroutine _sizeRoutine;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();
        }

        public void MoveToCamPos(CameraPos2D camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = DefaultDeltaType, Action onFinish = null)
        {
            StartMoveRoutine(MoveToPositionRoutine(() => camPos.transform.position, duration, easing, deltaType, onFinish));
            StartSizeRoutine(MoveToSizeRoutine(() => camPos.OrthographicSize, duration, easing, deltaType));
        }
        public void MoveToCamPosSin(CameraPos2D camPos, float duration, AWDelta.DeltaType deltaType = DefaultDeltaType)
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

        private void SetPosition(Vector3 position)
        {
            transform.position = transform.position.SetXY(position);
        }
        private void SetSize(float size)
        {
            _camera.orthographicSize = size;
        }
    }
}
