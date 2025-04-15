using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [ExecuteInEditMode]
    public class FlickeringLight : LightControls
    {
        
        [SerializeField]
        private AnimationCurve _testCurve;

        private float _timeDelta;

        private void Update()
        {
            _timeDelta += Time.deltaTime;
            _timeDelta = _timeDelta % 1;

            _light.intensity = Mathf.PerlinNoise1D(_timeDelta); //_testCurve.Evaluate(_timeDelta) * _intensity;
        }
    }
}
