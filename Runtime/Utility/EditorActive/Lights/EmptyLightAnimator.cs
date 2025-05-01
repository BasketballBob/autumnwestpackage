using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class EmptyLightAnimator : LightAnimator
    {
        [SerializeField]
        private float _intensity;
        //private float _initialRange;

        protected override void Start()
        {
            base.Start();

            _light.intensity = _intensity;
        }

        protected override void OnDeltaChange(float delta)
        {
            _light.intensity = _intensity * EnabledDelta;
        }
    }
}
