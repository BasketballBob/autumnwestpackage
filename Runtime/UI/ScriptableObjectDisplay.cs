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
        private TMP_Text _text;

        [SerializeField]
        private AWScriptableObject _scriptableObject;
        [SerializeField]
        private bool _useFormat = false;
        [SerializeField] [ShowIf("_useFormat")]
        private string _format;

        private void OnEnable()
        {
            _scriptableObject.OnValueChanged += UpdateDisplay;

            UpdateDisplay();
        }

        private void OnDisable()
        {
            _scriptableObject.OnValueChanged -= UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            _text.text = _scriptableObject.ToString(_useFormat ? _format : null);
        }
    }
}
