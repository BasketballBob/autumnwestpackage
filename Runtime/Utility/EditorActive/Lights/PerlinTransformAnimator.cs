using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class PerlinTransformAnimator : PerlinNoiseAnimator
    {
        [SerializeField] [Required]
        private Transform _anchor;
        [SerializeField]
        private Vector3 _positionRange = Vector3.zero;
        [SerializeField]
        private Vector3 _scaleRange = Vector3.zero;
        [SerializeField]
        private Vector3 _rotationRange = Vector3.zero;

        protected override void OnDeltaChange(float delta)
        {
            //float perlin = Mathf.Lerp(-1, 1, GetPerlinNoise1D(delta));
            Vector3 perlinVector = Vector3.one.AxisLerp(-Vector3.one, GetPerlinNoiseVector3(delta));
            Debug.Log($"WEEEE {perlinVector}");

            transform.position = _anchor.position + _positionRange.AxisMultiply(perlinVector);
            //transform.localScale = _anchor.localScale
            transform.eulerAngles = _anchor.eulerAngles + _rotationRange.AxisMultiply(perlinVector);
        }
    }
}
