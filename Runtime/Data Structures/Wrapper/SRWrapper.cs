using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Stand for SerializeReference Wrapper 
    /// Wrapper class that applies [SerializeReference] to stored value;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class SRWrapper<T>
    {
        [SerializeReference] [HideLabel] [InlineProperty]
        public T Value;
    }
}
