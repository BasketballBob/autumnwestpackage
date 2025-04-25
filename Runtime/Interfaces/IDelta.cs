using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public interface IDelta
    {
        public abstract float Delta { get; }
    }

    [System.Serializable]
    public abstract class IDeltaReference : IDelta
    {
        public abstract float Delta { get; }
    }

    [System.Serializable]
    public abstract class IDeltaReference<TData> : IDeltaReference
    {
        [SerializeField] 
        protected TData _data;
    }

    [System.Serializable]
    public class IDeltaFloatVariableReference : IDeltaReference<FloatVariable>
    {
        public override float Delta => Mathf.Clamp01(_data.RuntimeValue);
    }
}
