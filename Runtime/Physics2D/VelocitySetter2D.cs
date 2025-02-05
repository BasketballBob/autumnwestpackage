using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class VelocitySetter2D : MonoBehaviour
    {
        [SerializeField]
        private SpaceType _spaceType;
        [SerializeField]
        private Vector2 _setVelocity;

        private Rigidbody2D _rb;

        public void SetVelocity()
        {
            EnsureRigidbodyReference();
            
            switch (_spaceType)
            {
                case SpaceType.WorldSpace:
                    _rb.velocity = _setVelocity;
                    break;
                case SpaceType.LocalSpace:
                    _rb.velocity = transform.InverseTransformVector(_setVelocity);
                    break;
            }
        }

        private void EnsureRigidbodyReference()
        {
            if (_rb != null) return;
            _rb = GetComponent<Rigidbody2D>();
        }
    }
}
