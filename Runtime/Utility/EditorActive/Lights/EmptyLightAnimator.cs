using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class EmptyLightAnimator : LightAnimator
    {
        [Header("Light variables")]
        [SerializeField]
        private float _intensity;
        [SerializeField]
        private Color _color = Color.white;
        
        public float Intensity { get { return _intensity; } set { _intensity = value; }}
        public Color Color { get { return _color; } set { _color = value; }}

        protected override void Start()
        {
            base.Start();

            _light.intensity = _intensity;
            _light.color = _color;
        }

        protected override void OnDeltaChange(float delta)
        {
            _light.intensity = _intensity * EnabledDelta;
            _light.color = _color;
        }
    }
}
