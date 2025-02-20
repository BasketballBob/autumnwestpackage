using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class PriorityList<TItem> where TItem : IComparable
    {
        public List<TItem> Items = new List<TItem>();
        public TItem PullItem(Func<TItem, bool> validateFunc)
        {
            if (Items.IsNullOrEmpty()) return default;
            TItem highestItem = Items[0];

            for (int i = 1; i < Items.Count; i++)
            {
                if (validateFunc(Items[i])) continue;
                if (Items[i].CompareTo(highestItem) > 0)
                {
                    highestItem = Items[i];
                }
            }

            if (!validateFunc(highestItem)) return default;
            return highestItem;
        }
    }
}
