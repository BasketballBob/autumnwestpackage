using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AWP
{
    public class PaperUIFX : UIFX
    {
        [SerializeField]
        private float _hoverSpeed = 50;
        [SerializeField]
        private float _gravity = 100;
        [SerializeField]
        private float _angularDrag = 3;

        private float _angle;
        private float _angularVelocity;

        public void ApplyAngularVelocity(float angularVelocity)
        {
            _angularVelocity += angularVelocity;
            StartAnimationRoutines();
        }

        public void Test(float weiner, float wiener2)
        {
            
        }

        protected override void FXReset()
        {
            
        }

        protected override void FXUpdate(float deltaTime)
        {
            //Debug.Log($"FX UPDATE {Vector2.Dot(_angle.GetAngleVector(), Vector2.down)}");

            _angle += _angularVelocity * deltaTime;
            _rect.eulerAngles = _rect.eulerAngles.SetZ(_angle);
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            //Debug.Log($"FX FIXED UPDATE");

            if (_isHighlighted)
            {
                //_angularVelocity += MouseDelta.x * _hoverSpeed * deltaTime;
                _angularVelocity += MouseVelocity.x * Vector2.Dot(_angle.GetAngleVector(), Vector2.right) * _hoverSpeed * deltaTime;
                _angularVelocity += MouseVelocity.y * Vector2.Dot(_angle.GetAngleVector(), Vector2.up) * _hoverSpeed * deltaTime;
            }

            _angularVelocity -= _gravity * AWUnity.SignWithZero(_angle)
                * Mathf.Abs(Vector2.Dot(_angle.GetAngleVector(), Vector2.down));

            _angularVelocity *= (1 - Time.fixedDeltaTime * _angularDrag);
        }
    }
}