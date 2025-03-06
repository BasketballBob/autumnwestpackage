using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class ValueObject<T> : ValueObject
    {
        //[OdinSerialize, TypeFilter(nameof(GetTypes))]
        public T Value;
    }

    [System.Serializable]
    public class ValueObject 
    { 
        public static IEnumerable<Type> GetTypes()
        {
            yield return typeof(ValueObject<Vector2>);
            yield return typeof(ValueObject<Vector3>);
        }
    } 
}
