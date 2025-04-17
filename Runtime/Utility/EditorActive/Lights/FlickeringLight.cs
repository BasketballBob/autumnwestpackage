using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [ExecuteInEditMode]
    public class FlickeringLight : PerlinNoiseAnimator
    {
        [SerializeField]
        protected Light _light;
        [SerializeField]
        private float _minIntensity = 5;
        [SerializeField]
        private float _maxIntensity = 10;

        private void Reset()
        {
            if (GetComponent<Light>() != null) 
            {
                _light = GetComponent<Light>();
            }
        }
        
        protected override void OnDeltaChange(float delta)
        {
            _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, GetPerlinNoise1D(delta));
        }
    }
}
