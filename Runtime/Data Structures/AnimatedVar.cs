using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable] [InlineProperty]
    public struct AnimatedVar<TVariable>
    {
        [HideLabel]
        public TVariable Value;
        private TVariable _oldValue;

        public AnimatedVar(TVariable value)
        {
            Value = value;
            _oldValue = default;
        }

        /// <summary>
        /// Checks if the variable has changed and runs the provided action if true
        /// </summary>
        /// <param name="action"></param>
        public void RunOnValueChange(Action<TVariable> action)
        {
            if (ValueHasChanged())
            {
                action(Value);
            } 

            _oldValue = Value;
        }

        private bool ValueHasChanged()
        {
            if (Value is IEquatable<TVariable> && !Value.Equals(_oldValue)) 
                return true;
            if (Value is object && (Value as object) == (_oldValue as object))
                return true;

            return false;
        }
    }
}
