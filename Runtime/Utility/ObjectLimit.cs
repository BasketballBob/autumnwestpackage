using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    public class ObjectLimit<T> where T : Component
    {
        public static Action<T> DefaultRemoveFunction = x => GameObject.Destroy(x.gameObject);

        public Action<T> OnRemove;

        private int _maxCount;
        private List<T> _items = new List<T>();
        private Action<T> _removeItemFunc = DefaultRemoveFunction;

        public int CurrentCount => _items.Count;
        public int MaxCount => _maxCount;
        public Action<T> DefaultRemoveFunc => DefaultRemoveFunction;

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
            destroyTracker.OnInvoke.AddListener(() => 
            {
                if (_items.Contains(item)) _items.Remove(item);
            });
            
            _items.Add(item);

            while (_items.Count > _maxCount)
            {
                RemoveItem(_items[0]);
            }
        }

        protected virtual void RemoveItem(T item)
        {
            if (item == null) return;

            _items.Remove(item);
            _removeItemFunc(item);

            OnRemove?.Invoke(item);
        }

        protected virtual void RemoveAll()
        {
            while (_items.Count > 0)
            {
                RemoveItem(_items[0]);
            }
        }

        public void SetRemoveItemFunc(Action<T> func)
        {
            _removeItemFunc = func;
        }

        public void ResetRemoveItemFunc() => _removeItemFunc = DefaultRemoveFunc;
    }
}
