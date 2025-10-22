using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    [System.Serializable]
    public class ComponentPool<TComponent> where TComponent : Component
    {
        [SerializeField]
        [OnValueChanged("OnPrefabChange")]
        private TComponent _prefab;
        /// <summary>
        /// Determines whether prefab is an instance that should be added to the poolItems
        /// </summary>
        [SerializeField]
        private bool _prefabInScene;

        private MonoBehaviour _mono;
        private List<TComponent> _poolItems = new List<TComponent>();

        public int ActiveItemCount { get; private set; }
        public TComponent Prefab => _prefab;

        public void Initialize(MonoBehaviour mono)
        {
            _mono = mono;
            if (_prefabInScene)
            {
                AddObjectToPool(_prefab);
                if (ObjectIsActive(_prefab)) ActiveItemCount++;
            }
        }

        public TComponent PullObject()
        {
            TComponent pulledObject = _poolItems.FirstOrDefault(x => !ObjectIsActive(x));
            if (pulledObject == null) pulledObject = CreateObject();

            SetObjectActive(pulledObject, true);
            
            return pulledObject;
        }

        public void DisposeObject(TComponent obj)
        {
            SetObjectActive(obj, false);
        }

        protected virtual TComponent CreateObject()
        {
            TComponent newObject = GameObject.Instantiate(_prefab, _mono.transform);
            Debug.Log("CREATEN NEW OBJECT");
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

#if UNITY_EDITOR
        private void OnPrefabChange()
        {
            if (PrefabUtility.IsPartOfPrefabAsset(_prefab))
            {
                _prefabInScene = false;
            }
            else _prefabInScene = true;
        }
#endif
    }
}
