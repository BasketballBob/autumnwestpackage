using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable] [InlineProperty]
    public struct AnimatedVar<TVariable> where TVariable : IEquatable<TVariable>
    {
        [HideLabel]
        public TVariable Value;
        private TVariable _oldValue;

        /// <summary>
        /// Checks if the variable has changed and runs the provided action if true
        /// </summary>
        /// <param name="action"></param>
        public void RunOnValueChange(Action<TVariable> action)
        {
            if (!Value.Equals(_oldValue))
            {
                action(Value);
            }   

            _oldValue = Value;
        }
    }
}
