using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CullResetRigidbody2D : CullResetPosition
    {
        private Rigidbody2D _rb;

        protected virtual Vector2 ResetVelocity => Vector2.zero;
        protected virtual float ResetAngularVelocity => 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void Cull()
        {
            base.Cull();
            _rb.velocity = ResetVelocity;
            _rb.angularVelocity = ResetAngularVelocity;
        }
    }
}
