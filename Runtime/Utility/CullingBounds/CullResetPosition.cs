using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class CullResetPosition : CullingObject
    {
        private Vector2 _initialPosition;
        private Quaternion _initialRotation;

        protected virtual void Start()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        public override void Cull()
        {
            base.Cull();
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
        }
    }
}
