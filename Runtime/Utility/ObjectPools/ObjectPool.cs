using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    [System.Serializable]
    public abstract class ObjectPool<TObject> : MonoBehaviour where TObject : Component
    {
        [SerializeField] [SceneObjectsOnly]
        protected TObject _prefab;
        [SerializeField]
        protected int _minItemCount = 5;
        private List<TObject> _poolItems = new List<TObject>();

        protected List<TObject> Items => _poolItems;
        public int ActiveItemCount { get; private set; }

        private bool _initialized;

        protected virtual void Start()
        {
            CheckToInitialize();
        }

        /// <summary>
        /// Initializes the ObjectPool if it hasn't been already 
        /// (Useful for situations where the pool may be modified before it's start event occurs)
        /// </summary>
        protected void CheckToInitialize()
        {
            if (_initialized) return;

            // Add initial prefab object
            AddObjectToPool(_prefab);
            DisableObject(_prefab);

            // Create remaining minimal item count
            for (; _poolItems.Count <= _minItemCount;)
            {
                DisableObject(CreateObject());
            }

            _initialized = true;
        }

        public TObject PullObject()
        {
            for (int i = 0; i < _poolItems.Count; i++)
            {
                if (!ObjectIsActive(_poolItems[i]))
                {
                    EnableObject(_poolItems[i]);

                    return _poolItems[i];
                }
            }

            return CreateObject();
        }

        public void PullObject(Action<TObject> action)
        {
            action(PullObject());
        }

        public void DisposeObject(TObject obj)
        {
            if (obj == null) return;
            DisableObject(obj);
        }

        /// <summary>
        /// Sets active object count to "count", creating and disabling objects as necessary
        /// </summary>
        /// <param name="count"></param>
        public void SetActiveCount(int count)
        {
            // Create new items if necessary
            while (_poolItems.Count < count)
            {
                CreateObject();
            }

            // Set desired count active
            for (int i = 0; i < _poolItems.Count; i++)
            {
                if (i < count)
                {
                    EnableObject(_poolItems[i]);
                }
                else DisableObject(_poolItems[i]);
            }

            ActiveItemCount = count;
        }

        /// <summary>
        /// Helper function that calls SyncObjectValues on all active objects
        /// </summary>
        public void SyncActiveValues() => ModifyActiveObjects((x, y) =>
        {
            SyncObjectValues(x, y);
        });

        public void ModifyActiveObjects(Action<TObject, int> action)
        {
            for (int i = 0; i < Items.Count && i < ActiveItemCount; i++)
            {
                action(Items[i], i);
            }
        }

        /// <summary>
        /// Syncs the values on the provided item with index
        /// </summary>
        protected virtual void SyncObjectValues(TObject obj, int index) { }

        protected virtual TObject CreateObject()
        {
            TObject newObject = Instantiate(_prefab, transform);
            AddObjectToPool(newObject);

            return newObject;
        }

        protected virtual void AddObjectToPool(TObject obj)
        {
            _poolItems.Add(obj);
        }

        protected void EnableObject(TObject obj)
        {
            if (!ObjectIsActive(obj)) ActiveItemCount++;
            obj.gameObject.SetActive(true);
        }
        protected void DisableObject(TObject obj)
        {
            if (ObjectIsActive(obj)) ActiveItemCount--;
            obj.gameObject.SetActive(false);
        }
        protected abstract bool ObjectIsActive(TObject obj);
    }
}
