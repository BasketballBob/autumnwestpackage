using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public abstract class ScriptableVariable<T> : AWScriptableObject, ISerializationCallbackReceiver
    {
        public const string CreateFolderName = "Scriptable Variables/";

        public T InitialValue;

        public T RuntimeValue 
        {
            get 
            {
                return _runtimeValue;
            }
            set
            {
                _runtimeValue = value;
                OnValueChanged?.Invoke();
            }
        }
        [ShowInInspector] [ReadOnly()]
        protected T _runtimeValue;

        #if UNITY_EDITOR
            [Header("DEBUG")]
            [ShowInInspector]
            private T testValue;
            [Button()]
            private void SetTestValue() => RuntimeValue = testValue;
        #endif

        public void OnAfterDeserialize()
        {
            _runtimeValue = InitialValue;
        }

        public void OnBeforeSerialize() { }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (_runtimeValue is IFormattable)
            {
                return (_runtimeValue as IFormattable).ToString(format, formatProvider);
            }

            return _runtimeValue.ToString();
        }
    }
}
