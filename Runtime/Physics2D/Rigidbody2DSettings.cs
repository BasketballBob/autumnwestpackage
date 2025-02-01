using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public struct Rigidbody2DSettings
    {
        public static readonly Rigidbody2DSettings DefaultSettings = new Rigidbody2DSettings()
        {
            BodyType = RigidbodyType2D.Dynamic,
            PhysicsMaterial = null,
            Mass = 1,
            Drag = 1,
            AngularDrag = 0.05f,
            GravityScale = 1f,
            CollisionDetectionMode = CollisionDetectionMode2D.Discrete,
            Interpolation = RigidbodyInterpolation2D.None,
            Constraints = RigidbodyConstraints2D.None,
        };

        public RigidbodyType2D BodyType;
        public PhysicsMaterial2D PhysicsMaterial;
        public float Mass;
        public float Drag;
        public float AngularDrag;
        public float GravityScale;
        public CollisionDetectionMode2D CollisionDetectionMode;
        public RigidbodyInterpolation2D Interpolation;
        public RigidbodyConstraints2D Constraints;

        public Rigidbody2DSettings(Rigidbody2D rigidbody2D)
        {
            BodyType = rigidbody2D.bodyType;
            PhysicsMaterial = rigidbody2D.sharedMaterial;
            Mass = rigidbody2D.mass;
            Drag = rigidbody2D.drag;
            AngularDrag = rigidbody2D.angularDrag;
            GravityScale = rigidbody2D.gravityScale;
            CollisionDetectionMode = rigidbody2D.collisionDetectionMode;
            Interpolation = rigidbody2D.interpolation;
            Constraints = rigidbody2D.constraints;
        }

        public void ApplySettings(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.bodyType = BodyType;
            rigidbody2D.sharedMaterial = PhysicsMaterial;
            rigidbody2D.mass = Mass;
            rigidbody2D.drag = Drag;
            rigidbody2D.angularDrag = AngularDrag;
            rigidbody2D.gravityScale = GravityScale;
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode;
            rigidbody2D.interpolation = Interpolation;
            rigidbody2D.constraints = Constraints;
        }
    }
}
