using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class CardUIFX : UIFX
    {
        [SerializeField]
        private Vector2 _maxAngles = new Vector2(10, 10);

        // protected override void HighlightedUpdate()
        // {
        //     float xAngle = _maxAngles.x * MouseDelta.x;
        //     float yAngle = _maxAngles.y * MouseDelta.y;

        //     _rect.rotation = Quaternion.identity;
        //     _rect.Rotate(new Vector3(yAngle, xAngle));
            
        //     Debug.Log($"MOUSE DELTA {MouseDelta}");
        // }

        protected override void FXReset()
        {
            
        }

        protected override void FXUpdate(float deltaTime)
        {

        }

        protected override void FXFixedUpdate(float deltaTime)
        {

        }
    }
}
