using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class PerlinTransformAnimator : TransformAnimator
    {
        [SerializeField] 
        private float _perlinNoiseScale = 1;
        [SerializeField]
        private Vector3 _positionRange = Vector3.zero;
        [SerializeField]
        private Vector3 _scaleRange = Vector3.zero;
        [SerializeField]
        private Vector3 _rotationRange = Vector3.zero;

        protected override float Duration => base.Duration * _perlinNoiseScale;

        protected override void OnDeltaChange(float delta)
        {
            //float perlin = Mathf.Lerp(-1, 1, GetPerlinNoise1D(delta));
            Vector3 perlinVector = Vector3.one.AxisLerp(-Vector3.one, AWUnity.PerlinNoiseVector3(delta, _perlinNoiseScale));

            transform.position = _anchor.position + _positionRange.AxisMultiply(perlinVector);
            //transform.localScale = _anchor.localScale
            transform.eulerAngles = _anchor.eulerAngles + _rotationRange.AxisMultiply(perlinVector);
        }
    }
}
