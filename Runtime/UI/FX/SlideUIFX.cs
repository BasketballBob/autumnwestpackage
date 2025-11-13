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

        private Vector2 _velocity;
        private Vector2 _offset;


        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_rect.parent.TransformPoint(Vector2.zero) + _slideBounds.center, _slideBounds.size);
        }

        protected override void FXReset()
        {
            _offset = Vector2.zero;
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
            return false; // TEMPORARY TEST TEST TEST
        }

        private void SyncOffset()
        {
            if (_offset.x < _slideBounds.min.x || _offset.x > _slideBounds.max.x) _velocity = _velocity.SetX(0);
            if (_offset.y < _slideBounds.min.y || _offset.y > _slideBounds.max.y) _velocity = _velocity.SetY(0);

            _offset = _offset.Clamp(_slideBounds.min, _slideBounds.max);
            _rect.anchoredPosition = _offset;
        }
    }
}
