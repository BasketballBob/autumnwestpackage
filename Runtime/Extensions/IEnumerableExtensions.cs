using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    public static class IEnumerableExtensions
    {
        public static T PullItemRandom<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.PullRandomWeightedItem(x => 1);
        }

        /// <summary>
        /// Randomly pulls an item from a list based on weighted chance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="weightFunc"></param>
        /// <returns></returns>
        public static T PullRandomWeightedItem<T>(this IEnumerable<T> enumerable, Func<T, float> weightFunc)
        {
            float randomWeight = UnityEngine.Random.Range(0, GetWeightTotal(enumerable, weightFunc));
            float currentWeight = 0;

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    currentWeight += weightFunc(enumerator.Current);
                    if (randomWeight <= currentWeight)
                    {
                        return enumerator.Current;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the sum of all weight of contained items using the provided weightFunc
        /// Mostly helper function for randomly pulling items based on weight
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="weightFunc"></param>
        /// <returns></returns>
        public static float GetWeightTotal<T>(this IEnumerable<T> enumerable, Func<T, float> weightFunc)
        {
            float weightTotal = 0;

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext()) weightTotal += weightFunc(enumerator.Current);
            }

            return weightTotal;
        }

        /// <summary>
        /// Gets the item of the highest weight
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="weightFunc"></param>
        /// <returns></returns>
        public static T GetHighestWeightItem<T>(this IEnumerable<T> enumerable, Func<T, float> weightFunc)
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

        public static int GetItemCount<T>(this IEnumerable<T> enumerable)
        {
            int count = 0;
            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext()) count++;
            }
            return count;
        }
        //public static T
    }
}
