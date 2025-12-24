using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    public class SlideUIFX : UIFX
    {
        [SerializeField]
        private Bounds _slideBounds;
        [SerializeField]
        private float _slideSpeed = 20;
        [SerializeField]
        private float _slideDrag = 1;
        [SerializeField]
        private float _gravityMultiplier = 0;


        private float _bounciness = .2f;

        private Vector2 _velocity;
        private Vector2 _offset;


        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_rect.parent.TransformPoint(Vector2.zero) + _slideBounds.center, _slideBounds.size);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            StartAnimationRoutines();
        }

        /// <summary>
        /// Sets the offset relative to the mins and maxes of the bounds
        /// .5f .5f is the center
        /// </summary>
        /// <param name="delta"></param>
        public void SetOffsetDelta(Vector2 delta)
        {
            _offset = _slideBounds.min.AxisLerp(_slideBounds.max, delta);
            Debug.Log($"AXIS LERP {_slideBounds.min} {_slideBounds.max} {delta} {_offset}");
            SyncOffset();
        }

        protected override void FXReset()
        {
            //_offset = Vector2.zero;
        }

        protected override void FXUpdate(float deltaTime)
        {
            _offset += _velocity * _slideSpeed * deltaTime;
            
            SyncOffset();
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            // Mouse velocity
            if (_isHighlighted)
            {
                _velocity += MouseVelocity * deltaTime;
            }

            // Gravity
            if (_gravityMultiplier != 0)
            {
                _velocity += Physics2D.gravity * deltaTime;
            }

            // Drag
            _velocity *= (1 - deltaTime * _slideDrag);
        }

        protected override bool FXFinished()
        {
            if (_isHighlighted) return false;
            return _velocity.magnitude < 1; // TEST
        }

        private void SyncOffset()
        {
            if (_offset.x < _slideBounds.min.x || _offset.x > _slideBounds.max.x) _velocity = _velocity.SetX(-_velocity.x * _bounciness);
            if (_offset.y < _slideBounds.min.y || _offset.y > _slideBounds.max.y) _velocity = _velocity.SetY(-_velocity.y * _bounciness);

            _offset = _offset.Clamp(_slideBounds.min, _slideBounds.max);
            _rect.anchoredPosition = _offset;
        }
    }
}
