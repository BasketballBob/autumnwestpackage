using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class EmptyLightAnimator : LightAnimator
    {
        private float _initialIntensity;
        //private float _initialRange;

        protected override void Start()
        {
            base.Start();

            _initialIntensity = _light.intensity;
            //_initialRange = _light.range;
        }

        protected override void OnDeltaChange(float delta)
        {
            _light.intensity = _initialIntensity * EnabledDelta;
        }
    }
}
