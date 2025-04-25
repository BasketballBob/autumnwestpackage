using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class UIBar : AWUI
    {
        [OnValueChanged("SyncVisuals")]
        public float Delta = 1;
        [SerializeField]
        private RectTransform _rectTrans;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Image _mask;

        [SerializeReference]
        private DeltaVariable _optionalVariable;

        private void Reset()
        {
            _rectTrans = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (_optionalVariable != null) _optionalVariable.OnValueChanged += SyncVariable;
        }

        private void OnDisable()
        {
            if (_optionalVariable != null) _optionalVariable.OnValueChanged -= SyncVariable;
        }

        private void Start()
        {
            
        }

        private void SyncVisuals()
        {
            Delta = Mathf.Clamp01(Delta);

            _mask.rectTransform.sizeDelta = _mask.rectTransform.sizeDelta.SetX(-_rectTrans.sizeDelta.x * (1 - Delta));
            _image.rectTransform.position = _rectTrans.position;
        }

        private void SyncVariable()
        {
            Delta = _optionalVariable.Delta;
            SyncVisuals();
        }
    }
}
