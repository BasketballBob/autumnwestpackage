using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class SingleCoroutine
    {
        private MonoBehaviour _mono;
        private Coroutine _routine;

        public SingleCoroutine(MonoBehaviour mono)
        {
            _mono = mono;
        }

        public bool RoutineActive { get; private set; }
        public Coroutine Routine => _routine;

        public Coroutine StartRoutine(IEnumerator routine)
        {
            StopRoutine();
            return _mono.StartCoroutine(TrackingRoutine());

            IEnumerator TrackingRoutine()
            {
                RoutineActive = true;
                yield return routine;
                RoutineActive = false;
            }
        }

        public void StopRoutine()
        {
            if (_routine == null) return;
            _mono.StopCoroutine(_routine);
        }
    }
}
