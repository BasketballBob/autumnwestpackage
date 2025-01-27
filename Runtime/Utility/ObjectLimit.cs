using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class ObjectLimit<T> where T : Component
    {
        private int _maxCount;
        private Queue<T> _items = new Queue<T>();
        private Action<T> _removeItemFunc = x => GameObject.Destroy(x.gameObject);

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
            _items.Enqueue(item);

            while (_items.Count > _maxCount)
            {
                RemoveItem(_items.Dequeue());
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
                RemoveItem(_items.Dequeue());
            }
        }
    }
}
