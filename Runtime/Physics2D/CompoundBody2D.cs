using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AWP
{
    public class CompoundBody2D : MonoBehaviour
    {
        [SerializeField]
        private List<BodySegment> _segments = new List<BodySegment>();
        [SerializeField]
        private Transform _disconnectTransform;

        private Rigidbody2D _rb;

        public Rigidbody2D Rigidbody2D => _rb;
        public Transform DisconnectTransform { get => _disconnectTransform; set => _disconnectTransform = value; }
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
            _rb.BalanceCenterOfMass(_segments.Select(x => new Tuple<Transform, float>(x.Trans, x.Mass)).ToList());
        }

        public void SetWorldCenterOfMass(Vector3 worldPos)
        {
            _rb.centerOfMass = _rb.transform.InverseTransformPoint(worldPos); //Vector3.Cross(, _rb.transform.lossyScale);
        }

        public void DestroySegment(Transform transform, bool updateCenterOfMass = true)
        {
            BodySegment segment = RemoveSegment(transform, updateCenterOfMass);
            if (segment == null) return;

            Destroy(segment.Trans.gameObject);
        }

        public Rigidbody2D DisconnectSegment(Transform transform, bool updateCenterOfMass = true)
            => DisconnectSegment(RemoveSegment(transform, updateCenterOfMass));
        public Rigidbody2D DisconnectSegment(string name, bool updateCenterOfMass = true)
            => DisconnectSegment(RemoveSegment(name, updateCenterOfMass));
        private Rigidbody2D DisconnectSegment(BodySegment segment)
        {
            if (segment == null) return null;

            Rigidbody2D rb = segment.Trans.gameObject.AddComponent<Rigidbody2D>();
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.mass = segment.Mass;
            rb.velocity = _rb.velocity;
            rb.angularVelocity = _rb.angularVelocity;
            rb.gravityScale = _rb.gravityScale;
            rb.transform.SetParent(_disconnectTransform);

            return rb;
        }
        public void DisconnectSegment(Transform transform) => DisconnectSegment(transform, updateCenterOfMass: true);

        public void DisconnectSegmentAndIgnoreCollision(Transform transform)
        {
            Rigidbody2D rb = DisconnectSegment(transform, updateCenterOfMass: true);
            AWPhysics2D.IgnoreRigidbodyCollision(_rb, rb);
        }

        public void AddSegment(Transform transform, float mass, bool updateCenterOfMass = true)
        {
            BodySegment newSegment = new BodySegment()
            {
                Trans = transform,
                Mass = mass
            };

            _segments.Add(newSegment);
            if (updateCenterOfMass) UpdateCenterOfMass();
        }

        public BodySegment RemoveSegment(Transform transform, bool updateCenterOfMass = true)
            => RemoveSegment(GetSegment(transform), updateCenterOfMass);
        public BodySegment RemoveSegment(string name, bool updateCenterOfMass = true)
            => RemoveSegment(GetSegment(name), updateCenterOfMass);
        private BodySegment RemoveSegment(BodySegment segment, bool updateCenterOfMass = true)
        {
            if (segment == null) return null;

            _segments.Remove(segment);
            if (updateCenterOfMass) UpdateCenterOfMass();
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

        private BodySegment GetSegment(string name)
        {
            return _segments.First(x => name == x.Trans.name);
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
