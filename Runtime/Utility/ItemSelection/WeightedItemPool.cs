using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class WeightedItemPool<T> : WeightedItemPool
    {
        [SerializeField]
        protected List<WeightedItem> itemList = new List<WeightedItem>();

        [System.Serializable]
        protected class WeightedItem
        {
            public T Value;
            public float Weight = 1;
        }

        public T PullItem()
        {

            return PullItem(itemList, (x) => x.Weight).Value; // );
        }

        public T PullItemConditionally(Func<T, bool> func)
        {
            T pulledItem = default;
            if (!ContainsConditionalItem(func)) return pulledItem;

            do
            {
                pulledItem = PullItem();
                if (!func(pulledItem)) continue;
                return pulledItem;
            }
            while (true);
        }

        public bool ContainsConditionalItem(Func<T, bool> func)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (func(itemList[i].Value)) return true;
            }

            return false;
        }

        public List<T> GetValueList()
        {
            List<T> returnList = new List<T>();

            for (int i = 0; i < itemList.Count; i++)
            {
                returnList.Add(itemList[i].Value);
            }

            return returnList;
        }

        public IEnumerable<T> GetEnumerable()
        {
            return itemList.Select(x => x.Value);
        }

        private float GetWeightTotal()
        {
            return GetWeightTotal(itemList, (x) => x.Weight);
        }

        public bool IsEmpty => itemList.Count <= 0;
        public int Count => itemList.Count;
    }

    public abstract class WeightedItemPool
    {
        /// <summary>
        /// Pulls an item from a list using a func to determine item weight
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="weightFunc"></param>
        /// <returns></returns>
        public static TItem PullItem<TItem>(List<TItem> list, Func<TItem, float> weightFunc)
        {
            float randomWeight = UnityEngine.Random.Range(0, GetWeightTotal(list, weightFunc));
            float currentWeight = 0;

            for (int i = 0; i < list.Count; i++)
            {
                currentWeight += weightFunc(list[i]);

                if (randomWeight <= currentWeight)
                {
                    return list[i];
                }
            }

            throw new System.Exception("ERROR: Was unable to find an item!");
        }

        public static float GetWeightTotal<TItem>(List<TItem> list, Func<TItem, float> weightFunc)
        {
            float weightTotal = 0;

            for (int i = 0; i < list.Count; i++)
            {
                weightTotal += weightFunc(list[i]);
            }

            return weightTotal;
        }
    }
}
