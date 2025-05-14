using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AWP
{
    public class ScriptableObjectDisplay : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text _text;

        [SerializeField]
        protected AWScriptableObject _scriptableObject;
        [SerializeField]
        private bool _useFormat = false;
        [SerializeField] [ShowIf("_useFormat")]
        private string _format;
        [SerializeField]
        private string _openText;
        [SerializeField]
        private string _closeText;

        private void OnEnable()
        {
            _scriptableObject.OnValueChanged += UpdateDisplay;

            UpdateDisplay();
        }

        private void OnDisable()
        {
            _scriptableObject.OnValueChanged -= UpdateDisplay;
        }

        protected virtual void UpdateDisplay()
        {
            _text.text = _openText + _scriptableObject.ToString(_useFormat ? _format : null) + _closeText;
        }
    }
}
