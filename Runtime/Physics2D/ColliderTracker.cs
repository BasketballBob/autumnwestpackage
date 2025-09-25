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

        public Action<T> OnEnterCollider;
        public Action<T> OnExitCollider;

        public List<T> Colliders => _colliderList;

        protected virtual void OnColliderEnter(T col)
        {
            _colliderList.Add(col);
            OnEnterCollider?.Invoke(col);
            OnColliderListChange();
        }

        protected virtual void OnColliderExit(T col)
        {
            _colliderList.Remove(col);
            OnExitCollider?.Invoke(col);
            OnColliderListChange();
        }

        protected virtual void OnColliderListChange()
        {

        }
    }
}
