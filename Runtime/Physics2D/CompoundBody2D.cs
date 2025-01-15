using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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

        private List<Collider2D> AttachedColliders 
        {
            get
            {
                return _segments.Where(x => x.Col != null).Select(x => x.Col).ToList();
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (_segments.Count <= 0) return;
            InitializeSegments();
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

        public void DestroySegment(Transform transform)
        {
            BodySegment segment = RemoveSegment(transform);
            if (segment == null) return;

            Destroy(segment.Trans.gameObject);
        }

        public void DisconnectSegment(Transform transform)
        {
            BodySegment segment = RemoveSegment(transform);
            if (segment == null) return;

            Rigidbody2D rb = segment.Trans.gameObject.AddComponent<Rigidbody2D>();
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.mass = segment.Mass;
            rb.velocity = _rb.velocity;
            rb.angularVelocity = _rb.angularVelocity;
            rb.transform.SetParent(null);
        }

        public BodySegment RemoveSegment(Transform transform)
        {
            BodySegment segment = GetSegment(transform);
            if (segment == null) return null;

            //Rigidbody2D rb = segment.Trans.gameObject.AddComponent<Rigidbody2D>();
            //rb.mass = segment.Mass;
            //rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            //rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            _segments.Remove(segment);
            UpdateCenterOfMass();
            return segment;
        }

        public List<Collider2D> OverlapColliders(ContactFilter2D contactFilter2D)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            AttachedColliders.ForEach(x => 
            {
                Debug.Log("TOUCH ATTACHED COLLIDERS " + x.name);
                Collider2D[] results = new Collider2D[8];
                Physics2D.OverlapCollider(x, contactFilter2D, results);
                results.ForEach(x =>
                {
                    if (x == null) return;
                    Debug.Log("COLLISION " + x.name);
                    if (colliders.Contains(x)) return;
                    colliders.Add(x);
                });
            });

            Debug.Log("OVERLAP " + colliders.Count);

            return colliders;
        }

        private BodySegment GetSegment(Transform transform)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                if (_segments[i].Trans == transform)
                {
                    return _segments[i];
                }
            }

            return null;
        }

        private void InitializeSegments()
        {
            _segments.ForEach(x => x.Initialize());
        }

        [System.Serializable]
        public class BodySegment
        {
            public Transform Trans;
            [ReadOnly]
            public Collider2D Col;
            public float Mass;

            public void Initialize()
            {
                Col = Trans.GetComponent<Collider2D>();
            }
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
