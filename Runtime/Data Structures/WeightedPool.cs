using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class WeightedPool<TItem>
    {
        [SerializeField]
        private List<WeightedItem> _items = new List<WeightedItem>();

        public WeightedItem this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }
        public int Count => _items.Count;
        public float TotalWeight 
        {
            get 
            {
                float total = 0;
                _items.ForEach((x) => total += x.Weight);
                return total;
            }
        }

        public TItem PullItem()
        {
            float pulledWeight = TotalWeight * AWRandom.Range01();
            float currentWeight = 0;

            for (int i = 0; i < _items.Count; i++)
            {
                currentWeight += _items[i].Weight;
                if (pulledWeight <= currentWeight)
                {
                    return _items[i].Item;
                }
            }

            throw new IndexOutOfRangeException();
        }

        public void AddItem(TItem item, float weight)
        {
            _items.Add(new WeightedItem(item, weight));
        }

        public void Clear() => _items.Clear();
        
        [System.Serializable]
        public struct WeightedItem
        {
            public TItem Item;
            public float Weight;

            public WeightedItem(TItem item, float weight)
            {
                Item = item;
                Weight = weight;
            }
        }
    }
}
