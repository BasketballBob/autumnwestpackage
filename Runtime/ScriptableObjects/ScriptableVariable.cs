using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public abstract class ScriptableVariable<T> : AWScriptableObject, ISaveable<T>, ISerializationCallbackReceiver
    {
        public T InitialValue;

        public virtual T RuntimeValue
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
        [ShowInInspector][ReadOnly()]
        protected T _runtimeValue;

#if UNITY_EDITOR
        [Header("DEBUG")]
        [ShowInInspector]
        private T testValue;
        [Button()]
        private void SetTestValue() => RuntimeValue = testValue;
#endif

        protected override void OnAfterDeserialize()
        {
            _runtimeValue = InitialValue;
        }

        protected override void OnBeforeSerialize() { }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (_runtimeValue is IFormattable)
            {
                return (_runtimeValue as IFormattable).ToString(format, formatProvider);
            }

            return _runtimeValue.ToString();
        }

        #region ISaveable
        public T GetSaveData()
        {
            return _runtimeValue;
        }

        public void LoadSaveData(T loadData)
        {
            _runtimeValue = loadData;
        }
        #endregion
    }
}
