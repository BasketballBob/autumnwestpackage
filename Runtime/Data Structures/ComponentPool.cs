using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class ComponentPool<TComponent> where TComponent : Component
    {
        [SerializeField]
        private TComponent _prefab;
   
        private MonoBehaviour _mono;
        private List<TComponent> _poolItems;

        public int ActiveItemCount { get; private set; }

        public void Initialize(MonoBehaviour mono)
        {
            _mono = mono;
        }

        public TComponent PullObject()
        {
            TComponent pulledObject = _poolItems.First(x => !ObjectIsActive(x));
            SetObjectActive(pulledObject, true);
            if (pulledObject == null) pulledObject = CreateObject();

            return pulledObject;
        }
        
        public void DisposeObject(TComponent obj)
        {
            SetObjectActive(obj, false);
        }

        protected virtual TComponent CreateObject()
        {
            TComponent newObject = Object.Instantiate(_prefab, _mono.transform);
            AddObjectToPool(newObject);

            return newObject;
        }

        protected void AddObjectToPool(TComponent obj) => _poolItems.Add(obj);
        protected void SetObjectActive(TComponent obj, bool active)
        {
            if (active != ObjectIsActive(obj))
            {
                if (active) ActiveItemCount++;
                else ActiveItemCount--;
            }

            obj.gameObject.SetActive(active);
        }
        protected bool ObjectIsActive(TComponent obj) => obj.gameObject.activeSelf;
    }
}
