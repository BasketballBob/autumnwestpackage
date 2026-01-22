using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class AWSlider : AWUI
    {
        [SerializeField]
        protected Slider _slider;

        private void Reset()
        {
            _slider = GetComponent<Slider>();
        }

        protected virtual void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnSliderChange);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderChange);
        }

        protected virtual void OnSliderChange(float newValue)
        {

        }
    }
}
