using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AWP
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CompoundBody2D : MonoBehaviour
    {
        [SerializeField]
        private List<BodySegment> _segments = new List<BodySegment>();

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (_segments.Count <= 0) return;
            UpdateCenterOfMass();
        }   

        public void SetCollidersActive(bool enabled)
        {
            _segments.ForEach((x) => 
            {
                Collider2D col = x.Trans.GetComponent<Collider2D>();
                if (col == null) return;

                col.enabled = enabled;
            });
        }

        public void UpdateCenterOfMass()
        {
            Vector2 centerOfMass = Vector2.zero;
            float totalMass = 0;

            foreach (BodySegment segment in _segments)
            {
                centerOfMass += (Vector2)(segment.Trans.position - _rb.transform.position) * segment.Mass;
                totalMass += segment.Mass;
            }

            _rb.centerOfMass = centerOfMass / totalMass;
            _rb.mass = totalMass;
        }

        public void SetWorldCenterOfMass(Vector3 worldPos)
        {
            _rb.centerOfMass = _rb.transform.InverseTransformPoint(worldPos); //Vector3.Cross(, _rb.transform.lossyScale);
        }

        [System.Serializable]
        public class BodySegment
        {
            public Transform Trans;
            public float Mass;
        }

        #if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!Application.isPlaying) return;

                Gizmos.color = AWUnity.SetAlpha(Color.red, .5f);
                Gizmos.DrawCube(_rb.transform.TransformPoint(_rb.centerOfMass), Vector2.one * .5f);
            }
        #endif
    }
}
