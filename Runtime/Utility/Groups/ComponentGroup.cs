using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    [ExecuteAlways]
    public class ComponentGroup<TComponent> : Group<TComponent> where TComponent : Component
    {
        [Button]
        protected void Reset()
        {
            SyncChildren();
        }

        public virtual void SyncChildren()
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "SyncChildren");
            #endif

            transform.TraverseSelfAndChildren(x => 
            {
                TComponent component = x.GetComponent<TComponent>();
                if (component == null) return;
                if (x.GetComponent<IgnoreGroup>() != null) return;
                if (_items.Contains(component)) return;
                
                _items.Add(component);
            });

            _items.ClearNullValues();
            
        }

        protected override bool ItemIsNull(TComponent item)
        {
            return item == null;
        }
    }
}
