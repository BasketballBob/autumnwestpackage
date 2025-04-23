using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AWP
{
    public abstract class ScriptableVariableSetter<TVariable> : MonoBehaviour where TVariable : IComparable<TVariable>
    {
        public ScriptableVariable<TVariable> Variable;
        public TVariable Accessor;

        private TVariable _oldValue;

        protected virtual void Start()
        {
            Variable.RuntimeValue = Accessor;
            _oldValue = Accessor;
        }

        protected virtual void LateUpdate()
        {
            CheckToUpdate();
            SyncOldValues();
        }

        private void CheckToUpdate()
        {
            if (_oldValue.Equals(Accessor)) return; 

            UpdateVariable();
        }

        protected virtual void SyncOldValues()
        {
            _oldValue = Accessor;
        }

        protected virtual void UpdateVariable()
        {
            Variable.RuntimeValue = _oldValue;
        }
    }
}
