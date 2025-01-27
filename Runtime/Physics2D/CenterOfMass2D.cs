using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CenterOfMass2D : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _centerOfMass;

        private Rigidbody2D _rb;

        public Vector2 CenterOfMass 
        { 
            get { return _centerOfMass; } 
            set 
            { 
                _centerOfMass = value; 
                _rb.centerOfMass = _centerOfMass;
            } 
        }

        protected virtual void OnEnable()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            CenterOfMass = _centerOfMass;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_rb.transform.TransformPoint(_centerOfMass), .1f);
        }
    }
}
