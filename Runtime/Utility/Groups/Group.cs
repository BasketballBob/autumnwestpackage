using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class Group<T> : MonoBehaviour
    {
        [SerializeField]
        protected List<T> _items = new List<T>();

        public void ModifyAll(Action<T> action)
        {
            _items.ForEach(x => action(x));
        }
    }
}
