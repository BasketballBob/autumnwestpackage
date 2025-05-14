using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    public class Group<T> : MonoBehaviour
    {
        [SerializeField]
        protected List<T> _items = new List<T>();

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public void ModifyAll(Action<T> action)
        {
            #if UNITY_EDITOR
            Undo.SetCurrentGroupName("ModifyAll");
            #endif

            for (int i = 0; i < _items.Count; i++)
            {
                // Cull null items
                if (ItemIsNull(_items[i]))
                {
                    _items.RemoveAt(i);
                    i--;
                    continue;
                }

                #if UNITY_EDITOR
                if (_items[i] is UnityEngine.Object) 
                {
                    if (_items[i] as UnityEngine.Object != null)
                    {
                        Undo.RecordObject(_items[i] as UnityEngine.Object, "ModifyAll");
                    }
                }
                #endif

                action(_items[i]);
            }
        }

        protected virtual bool ItemIsNull(T item)
        {
            return item == null;
        }
    }
}
