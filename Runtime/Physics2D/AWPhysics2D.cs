using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWPhysics2D
    {
        public static void Explosion(Vector2 point, float radius, float magnitude) => Explosion(point, radius, magnitude, ~0);
        public static void Explosion(Vector2 point, float radius, float magnitude, LayerMask layerMask, Action<Rigidbody2D, Vector2> hitAction = null)
        {
            Collider2D[] colArray = Physics2D.OverlapCircleAll(point, radius, layerMask);
            Debug.Log("POINT " + point + " " + radius);
            Debug.DrawLine(point, point - Vector2.right * radius, Color.red, 1);

            foreach (Collider2D col in colArray)
            {
                Rigidbody2D rb = col.attachedRigidbody;
                if (rb == null) continue;

                Vector2 difference = rb.position - point;
                if (difference.magnitude == 0) return;

                Vector2 appliedForce = difference.normalized * magnitude;
                Debug.Log("COL ARRAY " + col.name + " " + magnitude + " " + difference + " " + appliedForce);
                rb.AddForceAtPosition(appliedForce, col.ClosestPoint(point), ForceMode2D.Impulse);
                hitAction?.Invoke(rb, appliedForce);
            }
        }
    
        /// <summary>
        /// Use AddTorque() to face a specific angle. For 2D physiscs. Should be called in every FixedUpdate() frame.
        /// Ripped from: https://discussions.unity.com/t/rotate-a-2d-rigidbody-to-a-desired-angle-using-addtorque/157633/4
        /// </summary>
        /// <param name="currentVec"> vector representing the direction we are currently pointing at. (transform.right) </param>
        /// <param name="targetVec"> vector representing the direction we want to point at. </param>
        /// <param name="rb"> Rigidbody to affect. </param>
        /// <param name="maxTorque"> Max torque to apply. </param>
        /// <param name="torqueDampFactor"> Damping factor to avoid undershooting. </param>
        /// <param name="offsetForgive"> Stop applying force when the angles are within this threshold (default 0). </param>
        public static void TorqueTo(this Rigidbody2D rb, float angleDifference, float maxTorque, float torqueDampFactor, float offsetForgive = 0)
        {
            // if (Mathf.Abs(angleDifference) < offsetForgive) return;

            // float torqueToApply = maxTorque * angleDifference / 180f;
            // torqueToApply -= rb.angularVelocity * torqueDampFactor;
            // rb.AddTorque(torqueToApply, ForceMode2D.Force);

            throw new NotImplementedException();
        }   

        #region Collider2D

        #endregion
    }
}
