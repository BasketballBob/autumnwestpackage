using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class BoolVariableEvents : MonoBehaviour
    {
        [SerializeField]
        private BoolVariable _boolVariable;
        [SerializeField]
        private UnityEvent _onSetTrue;
        [SerializeField]
        private UnityEvent _onSetFalse;

        private void OnEnable()
        {
            _boolVariable.OnValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            _boolVariable.OnValueChanged -= OnValueChanged;
        }

        private void OnValueChanged()
        {
            if (_boolVariable.RuntimeValue) _onSetTrue.Invoke();
            else _onSetFalse.Invoke();
        }
    }
}
