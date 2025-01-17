using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class WeightedItemPool<T>
    {
        [SerializeField]
        protected List<WeightedItem> itemList = new List<WeightedItem>();

        public int CurrentIndex { get; protected set; }

        [System.Serializable]
        protected class WeightedItem
        {
            public T Value;
            public float Weight = 1;
        }

        public T PullItem()
        {
            float randomWeight = UnityEngine.Random.Range(0, GetWeightTotal());
            float currentWeight = 0;

            for (int i = 0; i < itemList.Count; i++)
            {
                currentWeight += itemList[i].Weight;

                if (randomWeight <= currentWeight)
                {
                    CurrentIndex = i;
                    return itemList[i].Value;
                }
            }

            throw new System.Exception("ERROR: Was unable to find an item!");
        }

        public T PullItemConditionally(Func<T, bool> func)
        {
            T pulledItem = default;

            do
            {
                pulledItem = PullItem();
                if (!func(pulledItem)) continue;
                return pulledItem;
            }   
            while (true);
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

        private float GetWeightTotal()
        {
            float weightTotal = 0;

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] == null) continue;

                weightTotal += itemList[i].Weight;
            }

            return weightTotal;
        }

        public bool IsEmpty => itemList.Count <= 0;
        public int Count => itemList.Count;
    }
}
