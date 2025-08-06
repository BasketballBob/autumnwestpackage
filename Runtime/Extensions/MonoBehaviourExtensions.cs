using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class MonoBehaviourExtensions
    {
        

        #region Coroutines
        public static Coroutine RepeatCoroutineIndefinitely(this MonoBehaviour mono, Func<IEnumerator> routineFunc, AWDelta.DeltaType deltaType, float rate)
        {
            return mono.StartCoroutine(RepeatIndefinitely(routineFunc, deltaType, rate));
        }
        public static Coroutine RepeatCoroutineIndefinitely(this MonoBehaviour mono, Action action, AWDelta.DeltaType deltaType, float rate)
        {
            return mono.StartCoroutine(RepeatIndefinitely(() => ActionCallRoutine(action), deltaType, rate));
        }

        public static IEnumerator ActionCallRoutine(Action action)
        {
            action?.Invoke();
            yield break;
        }

        private static IEnumerator RepeatIndefinitely(Func<IEnumerator> routineFunc, AWDelta.DeltaType deltaType, float rate)
        {
            return Routine();

            IEnumerator Routine()
            {
                while (true)
                {
                    yield return routineFunc();
                    yield return AWDelta.WaitForSeconds(deltaType, rate);
                }
            }
        }
        #endregion
    }
}
