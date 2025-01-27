using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TriggerPusher : MonoBehaviour
    {
        [SerializeField]
        protected float _forceMultiplier = 1;

        private Rigidbody2D _rb;

        protected virtual void OnEnable()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody == null) return;
            Push(other.attachedRigidbody);
        }

        private void Push(Rigidbody2D other)
        {
            other.AddForceAtPosition(_rb.velocity * _forceMultiplier, transform.position, ForceMode2D.Impulse);
        }
    }
}
