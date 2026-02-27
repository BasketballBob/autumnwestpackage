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

        public static Coroutine DelayedActionRoutine(this MonoBehaviour mono, Action action, float delay, AWDelta.DeltaType deltaType = AWDelta.DeltaType.Update)
        {
            return mono.StartCoroutine(DelayRoutine());

            IEnumerator DelayRoutine()
            {
                yield return deltaType.WaitForSeconds(delay);
                action?.Invoke();
            }
        }

        /// <summary>
        /// Calls action after a single yield null delay 
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="action"></param>
        /// <param name="deltaType"></param>
        /// <returns></returns>
        public static Coroutine YieldNullDelayedActionRoutine(this MonoBehaviour mono, Action action, AWDelta.DeltaType deltaType)
        {
            return mono.StartCoroutine(YieldNullDelayRoutine());

            IEnumerator YieldNullDelayRoutine()
            {
                yield return deltaType.YieldNull();
                action?.Invoke();
            }
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
