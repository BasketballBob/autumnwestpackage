using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    public static class IEnumerableExtensions
    {
        public static T GetHighestWeight<T>(this IEnumerable<T> enumerable, Func<T, float> weightFunc)
        {
            T highestItem = default;
            float highestWeight = float.MinValue;

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (weightFunc(enumerator.Current) > highestWeight)
                    {
                        highestItem = enumerator.Current;
                        highestWeight = weightFunc(enumerator.Current);
                    }
                }
            }

            return highestItem;
        }

        
    }
}
