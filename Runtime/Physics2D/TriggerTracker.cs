using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class TriggerTracker<T> : MonoBehaviour
    {
        protected List<T> _collidersInside = new List<T>();

        public List<T> CollidersInside { get { return _collidersInside; } }
        public int ColliderCount => _collidersInside.Count;
        public bool ContainsColliders => _collidersInside.Count == 0;

        protected void RegisterOther(T other)
        {
            _collidersInside.Add(other);
        }

        protected void UnregisterOther(T other)
        {
            _collidersInside.Remove(other);
        }
    }
}
