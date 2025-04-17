using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class ComponentGroup<TComponent> : Group<TComponent> where TComponent : Component
    {
        [Button]
        protected void Reset()
        {
            transform.TraverseSelfAndChildren(x => 
            {
                TComponent component = x.GetComponent<TComponent>();
                if (component == null) return;
                if (_items.Contains(component)) return;
                
                _items.Add(component);
            });
        }
    }
}
