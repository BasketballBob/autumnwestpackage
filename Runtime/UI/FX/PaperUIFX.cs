using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
        [SerializeField]
        private bool _worldSpace = false;

        private float _angle;
        private float _angularVelocity;
        private float _initialEulerZ;

        protected override void Start()
        {
            base.Start();
            _initialEulerZ = _rect.eulerAngles.z;
        }

        public void ApplyAngularVelocity(float angularVelocity)
        {
            _angularVelocity += angularVelocity;
            StartAnimationRoutines();
        }

        protected override void FXReset()
        {
            
        }

        protected override void FXUpdate(float deltaTime)
        {
            //Debug.Log($"FX UPDATE {Vector2.Dot(_angle.GetAngleVector(), Vector2.down)}");

            _angle += _angularVelocity * deltaTime;
            _rect.eulerAngles = _rect.eulerAngles.SetZ(_initialEulerZ + _angle);
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            //Debug.Log($"FX FIXED UPDATE");

            if (_isHighlighted)
            {
                //_angularVelocity += MouseDelta.x * _hoverSpeed * deltaTime;

                Vector2 screenOffset;
                if (_worldSpace) screenOffset = MousePos - (Vector2)AWGameManager.AWCamera.Camera.WorldToScreenPoint(_rect.position);
                else screenOffset = MouseScreenOffset;

                float dot = Vector2.Dot(screenOffset.normalized, MouseVelocity.normalized.PerpendicularClockwise());
                _angularVelocity += dot * MouseVelocity.magnitude * _hoverSpeed * deltaTime;

                Debug.DrawRay(transform.position, screenOffset.normalized, Color.red);
                Debug.DrawRay(transform.position, MouseVelocity, Color.cyan);
                Debug.Log($"DOT PRODUCT {Vector2.Dot(Vector2.right, Vector2.up)} {Vector2.Dot(Vector2.right, Vector2.down)}");

                // _angularVelocity += MouseVelocity.x * Vector2.Dot(_angle.GetAngleVector(), Vector2.right) * _hoverSpeed * deltaTime;
                // _angularVelocity += MouseVelocity.y * Vector2.Dot(_angle.GetAngleVector(), Vector2.up) * _hoverSpeed * deltaTime;
            }

            _angularVelocity -= _gravity * AWUnity.SignWithZero(_angle)
                * Mathf.Abs(Vector2.Dot(_angle.GetAngleVector(), Vector2.down));

            _angularVelocity *= (1 - Time.fixedDeltaTime * _angularDrag);
        }
    }
}