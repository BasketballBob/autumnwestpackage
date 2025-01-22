using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class IEnumeratorExtensions
    {
        public static IEnumerator RepeatIndefinitely(this IEnumerator enumerator, AWDelta.DeltaType deltaType, float rate)
        {
            return Routine();
            
            IEnumerator Routine()
            {
                while (true)
                {
                    yield return enumerator;
                    yield return AWDelta.WaitForSeconds(AWDelta.DeltaType.FixedUpdate, rate);
                }
            }
        }
    }
}
