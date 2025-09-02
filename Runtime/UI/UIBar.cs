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
        [SerializeField]
        private float _delta = 1;
        [SerializeField]
        private RectTransform _rectTrans;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Image _mask;

        [SerializeReference]
        private DeltaVariable _optionalVariable;

        public float Delta => _delta;

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

        public void SetDelta(float delta)
        {
            _delta = delta;
            SyncVisuals();
        }

        private void SyncVisuals()
        {
            _delta = Mathf.Clamp01(_delta);

            _mask.rectTransform.sizeDelta = _mask.rectTransform.sizeDelta.SetX(-_rectTrans.sizeDelta.x * (1 - _delta));
            _image.rectTransform.position = _rectTrans.position;
        }

        private void SyncVariable()
        {
            _delta = _optionalVariable.Delta;
            SyncVisuals();
        }
    }
}
