using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public abstract class ObjectPool<TObject> : MonoBehaviour where TObject : Component
    {
        [SerializeField]
        protected TObject _prefab;
        [SerializeField]
        protected int _minItemCount = 5;
        protected List<TObject> _poolItems = new List<TObject>();

        public List<TObject> Items => _poolItems;

        protected virtual void Start()
        {
            _poolItems.Clear();

            // Add initial prefab object
            AddObjectToPool(_prefab);
            DisableObject(_prefab);

            // Create remaining minimal item count
            for (int i = 0; i < _minItemCount - 1; i++)
            {
                DisableObject(CreateObject());
            }
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
            DisableObject(obj);
        }

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
            obj.gameObject.SetActive(true);
        }
        protected void DisableObject(TObject obj)
        {
            obj.gameObject.SetActive(false);
        }
        protected abstract bool ObjectIsActive(TObject obj);
    }
}
