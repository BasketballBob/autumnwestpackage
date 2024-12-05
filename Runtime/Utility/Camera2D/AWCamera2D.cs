using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWCamera2D : MonoBehaviour
    {   
        private const AWDelta.DeltaType DefaultDeltaType = AWDelta.DeltaType.Update;

        private Coroutine _moveRoutine;
        private Coroutine _sizeRoutine;

        public void StartMoveRoutine(IEnumerator routine)
        {
            if (_moveRoutine != null) StopCoroutine(routine);
            _moveRoutine = StartCoroutine(routine);
        }

        public void MoveToCamPos(CameraPos2D camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            StartMovementRoutine(MoveToPositionRoutine(() => camPos.transform.position, duration, easing, deltaType));
        }
        public void MoveToCamPosSin(CameraPos2D camPos, float duration, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            MoveToCamPos(camPos, duration, EasingFunction.Sin, deltaType);
        }
        
        public void StartMovementRoutine(IEnumerator routine)
        {
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(routine);
        }
        private IEnumerator MoveToPositionRoutine(Func<Vector3> func, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            Vector3 startPos = transform.position;
            
            yield return AnimationFX.DeltaRoutine((delta) =>
            {
                SetPosition(startPos + (func() - startPos) * delta);
            }, duration, easing, deltaType);

            StartMovementRoutine(HoldPositionRoutine(func, deltaType));
        }
        private IEnumerator HoldPositionRoutine(Func<Vector3> func, AWDelta.DeltaType deltaType = DefaultDeltaType)
        {
            while (true)
            {
                SetPosition(func());
                yield return deltaType.YieldNull();
            }
        }

        //private IEnumerator SizeRoutine(Func<float> size, float duration, )

        private void SetPosition(Vector3 position)
        {
            transform.position = transform.position.SetXY(position);
        }
    }
}
