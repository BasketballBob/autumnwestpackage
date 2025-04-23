using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class LightAnimator : EditorActiveAnimator
    {
        [SerializeField]
        protected Light _light;

        private void Reset()
        {
            if (GetComponent<Light>() != null) 
            {
                _light = GetComponent<Light>();
            }
        }
    }
}
