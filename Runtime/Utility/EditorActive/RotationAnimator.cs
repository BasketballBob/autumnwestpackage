using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class RotationAnimator : EditorActiveAnimator
    {
        [SerializeField]
        private Vector3 _rotationEulers = new Vector3();

        //protected override ActiveType ActiveSetting => ActiveType.Speed;

        protected override void OnDeltaChange(float delta)
        {
            transform.Rotate(_rotationEulers * delta);
        }
    }
}
