using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;

namespace AWP
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Ensures that all values present in the ensureDict are present in the other
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="ensureDict"></param>
        public static void EnsureValues<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> ensureDict)
        {
            ensureDict.ForEach(x =>
            {
                if (dict.ContainsKey(x.Key))
                {
                    dict[x.Key] = x.Value;
                }
                else dict.Add(x.Key, x.Value);
            });
        }

        // NOT WORKING CURRENTLY
        // public static Dictionary<TKey, TValue> GetWithClearedNullValues<TKey, TValue>(this Dictionary<TKey, TValue> dict) where TValue : class
        // {
        //     List<TKey> keysToRemove = new List<TKey>();

        //     dict.ForEach(x =>
        //     {
        //         if (x.Value != null) return;
        //         keysToRemove.Add(x.Key);
        //         Debug.Log($"REMOVE KEY {x}");
        //     });

        //     keysToRemove.ForEach(x =>
        //     {
        //         dict.Remove(x);
        //     });

        //     return dict;
        // }
    }
}
