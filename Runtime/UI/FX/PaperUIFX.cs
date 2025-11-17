using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace AWP
{
    public class PaperUIFX : UIFX
    {
        private const float MinAngle = .1f;
        private const float MinAngularVelocity = .01f;

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


        protected override void OnEnable()
        {
            _initialEulerZ = _rect.eulerAngles.z;
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void ApplyAngularVelocity(float angularVelocity)
        {
            _angularVelocity += angularVelocity;
            StartAnimationRoutines();
        }

        protected override void FXReset()
        {
            _angle = 0;
            _angularVelocity = 0;

            FXUpdate(0);
        }

        protected override void FXUpdate(float deltaTime)
        {
            _angle += _angularVelocity * deltaTime;
            _rect.eulerAngles = _rect.eulerAngles.SetZ(_initialEulerZ + _angle);
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            if (_isHighlighted)
            {
                Vector2 screenOffset;
                if (_worldSpace) screenOffset = MousePos - (Vector2)AWGameManager.AWCamera.Camera.WorldToScreenPoint(_rect.position);
                else screenOffset = MouseScreenOffset;

                float dot = Vector2.Dot(screenOffset.normalized, MouseVelocity.normalized.PerpendicularClockwise());
                _angularVelocity += dot * MouseVelocity.magnitude * _hoverSpeed * deltaTime;
            }

            _angularVelocity -= _gravity * AWUnity.SignWithZero(_angle)
                * Mathf.Abs(Vector2.Dot(_angle.GetAngleVector(), Vector2.down));

            _angularVelocity *= (1 - deltaTime * _angularDrag);
        }

        protected override bool FXFinished()
        {
            if (_isHighlighted) return false;
            if (Mathf.Abs(_angle) > MinAngle) return false;
            if (Mathf.Abs(_angularVelocity) > MinAngularVelocity) return false;

            return true;
        }
    }
}