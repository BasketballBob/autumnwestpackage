using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace AWP
{
    public static class MonoBehaviourExtensions
    {
        #region Coroutines
            public static Coroutine RepeatCoroutineIndefinitely(this MonoBehaviour mono, IEnumerator routine, AWDelta.DeltaType deltaType, float rate)
            {
                return mono.StartCoroutine(routine.RepeatIndefinitely(deltaType, rate));
            }
            public static Coroutine RepeatCoroutineIndefinitely(this MonoBehaviour mono, Action action, AWDelta.DeltaType deltaType, float rate)
            {
                return mono.StartCoroutine(IEnumeratorExtensions.RepeatIndefinitely(ActionCallRoutine(action), deltaType, rate));
            }

            public static IEnumerator ActionCallRoutine(Action action)
            {
                action?.Invoke();
                yield break;
            }
        #endregion
    }
}
