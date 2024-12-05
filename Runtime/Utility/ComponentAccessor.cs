using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class ComponentAccessor<T> : MonoBehaviour
    {
        protected abstract T Component { get; }
    }
}
