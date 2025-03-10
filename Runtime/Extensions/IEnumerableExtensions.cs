using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace AWP
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Evaluates if any of the items fit the condition func
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="cond">Condition function</param>
        /// <returns></returns>
        public static bool ConditionAny<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (condition(enumerator.Current))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Evaluates if all of the items fit the condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static bool ConditionAll<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            return !enumerable.ConditionAny(x => !condition(x));
        }

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
