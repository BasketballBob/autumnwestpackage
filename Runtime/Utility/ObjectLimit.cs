using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    public class ObjectLimit<T> where T : Component
    {
        private int _maxCount;
        private List<T> _items = new List<T>();
        private Action<T> _removeItemFunc = x => GameObject.Destroy(x.gameObject);

        public int CurrentCount => _items.Count;
        public int MaxCount => _maxCount;

        public ObjectLimit(int maxCount) 
        { 
            _maxCount = maxCount;
        }

        public ObjectLimit(int maxCount, Action<T> removeFunc)
        {
            _maxCount = maxCount;
            _removeItemFunc = removeFunc;
        }

        public void AddItem(T item)
        {
            OnDestroyTracker destroyTracker = item.GetComponent<OnDestroyTracker>();
            if (destroyTracker == null) destroyTracker = item.gameObject.AddComponent<OnDestroyTracker>();
            destroyTracker.OnInvoke += () => 
            {
                if (_items.Contains(item)) _items.Remove(item);
            };
            
            _items.Add(item);

            while (_items.Count > _maxCount)
            {
                RemoveItem(_items[0]);
            }
        }

        protected virtual void RemoveItem(T item)
        {
            if (item == null) return;

            _removeItemFunc(item);
        }

        protected virtual void RemoveAll()
        {
            while (_items.Count > 0)
            {
                RemoveItem(_items[0]);
            }
        }
    }
}
