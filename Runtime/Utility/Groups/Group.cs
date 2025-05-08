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

        public void ModifyAll(Action<T> action)
        {
            Undo.SetCurrentGroupName("ModifyAll");
            
            _items.ForEach(x => 
            {
                #if UNITY_EDITOR
                if (x is UnityEngine.Object) 
                    Undo.RecordObject(x as UnityEngine.Object, "ModifyAll");
                #endif

                action(x);
            });
        }
    }
}
