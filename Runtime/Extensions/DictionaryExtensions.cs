using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
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
    }
}
