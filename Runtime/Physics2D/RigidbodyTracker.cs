using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class RigidbodyTracker<T> : MonoBehaviour
    {
        private List<T> _rigidbodyList = new List<T>();
        public Action<T> OnEnterRigidbody;
        public Action<T> OnExitRigidbody;

        public List<T> Rigidbodies => _rigidbodyList;

        protected virtual void OnRigidbodyEnter(T body)
        {
            if (_rigidbodyList.Contains(body)) return;

            _rigidbodyList.Add(body);
            OnEnterRigidbody?.Invoke(body);
            OnRigidbodyListChange();
        }

        protected virtual void OnRigidbodyExit(T body)
        {
            if (!_rigidbodyList.Contains(body)) return;

            _rigidbodyList.Remove(body);
            OnExitRigidbody?.Invoke(body);
            OnRigidbodyListChange();
        }

        protected virtual void OnRigidbodyListChange()
        {

        }

        /// <summary>
        /// Used to determine whether any given collider is valid for registering a Rigidbody
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        protected virtual bool ColliderIsValid(Collider2D col)
        {
            return true;
        }
    }
}
