using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TiltUIFX : UIFX
    {
        [SerializeField]
        private float _tiltSpeed = 720;
        [SerializeField]
        private float _returnGravity = 360;
        [SerializeField]
        private AngleRange _tiltRange = new AngleRange(0, 10);
        [SerializeField]
        private bool _reverseTilt;

        private float _defaultAngle;
        private float _angle;
        private float _angularVelocity;

        private float ReverseMultiplier => _reverseTilt ? -1 : 1;

        protected override void OnEnable()
        {
            _defaultAngle = _rect.localEulerAngles.z;

            base.OnEnable();
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawRay()
            AWGizmos.DrawAngleGizmo(transform.position, 
                _tiltRange.MinAngle * ReverseMultiplier, 
                _tiltRange.MaxAngle * ReverseMultiplier, 50);
        }

        protected override void FXReset()
        {
            _angle = 0;
            _angularVelocity = 0;
            SyncAngle();
        }

        protected override void FXUpdate(float deltaTime)
        {
            _angle += _angularVelocity * deltaTime;

            if (!_tiltRange.InRange(_angle))
            {
                _angle = _tiltRange.ClampAngle(_angle);
                _angularVelocity = 0;
            }

            SyncAngle();
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            if (_isHighlighted)
            {
                _angularVelocity += _tiltSpeed * deltaTime;
            }
            else _angularVelocity -= _returnGravity * deltaTime;
        }

        protected override bool FXFinished()
        {
            if (_isHighlighted) return false;
            if (_angularVelocity != 0) return false;
            if (AWUnity.SignWithZero(_returnGravity) > 0 && _angle != _tiltRange.MinAngle) return false;
            if (AWUnity.SignWithZero(_returnGravity) < 0 && _angle != _tiltRange.MaxAngle) return false;

            return true;
        }

        public void SetAngularVelocity(float angularVelocity)
        {
            _angularVelocity = angularVelocity;
            StartAnimationRoutines();
        }

        private void SyncAngle()
        {
            _rect.localEulerAngles = _rect.localEulerAngles.SetZ(_defaultAngle + _angle * ReverseMultiplier);
        }
    }
}
