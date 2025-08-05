using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Base class for systems tracking colliders inside
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ColliderTracker<T> : MonoBehaviour
    {
        private List<T> _colliderList = new List<T>();

        public List<T> Colliders => _colliderList;

        public virtual void OnColliderEnter(T col)
        {
            _colliderList.Add(col);
            OnColliderListChange();
        }

        public virtual void OnColliderExit(T col)
        {
            _colliderList.Remove(col);
            OnColliderListChange();
        }

        protected virtual void OnColliderListChange()
        {

        }
    }
}
