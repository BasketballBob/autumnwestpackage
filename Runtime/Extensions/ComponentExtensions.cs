using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class ComponentExtensions
    {
        public static TComponent EnsureComponent<TComponent>(this Component component) where TComponent : Component
        {
            if (component.GetComponent<TComponent>() == null) component.gameObject.AddComponent<TComponent>();
            return component.GetComponent<TComponent>();
        }

        public static TComponent GetRootComponent<TComponent>(this Component component) where TComponent : Component
        {
            return component.transform.GetComponentInRoot<TComponent>();
        }
    }
}
