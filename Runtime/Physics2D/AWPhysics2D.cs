using System;
using System.Collections;
using System.Collections.Generic;
using PlasticPipe.Tube;
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
                if (difference.magnitude == 0) continue;

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

        #region Calculations
            /// <summary>
            /// 
            /// Borrowed from here: https://www.youtube.com/watch?v=RpeRnlLgmv8
            /// </summary>
            /// <param name="rb"></param>
            /// <param name="pos"></param>
            /// <param name="velocity"></param>
            /// <param name="steps"></param>
            /// <returns></returns>
            public static Vector2[] PlotTrajectory(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps, float maxXDist)
            {
                Vector2[] results = new Vector2[steps];

                float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
                Vector2 gravityAccel = Physics2D.gravity * rb.gravityScale * timestep * timestep;

                float drag = 1f - timestep * rb.drag;
                Vector2 moveStep = velocity * timestep;

                for (int i = 0; i < steps; i++)
                {
                    results[i] = pos;
                    moveStep += gravityAccel;
                    moveStep *= drag;
                    pos += moveStep;

                    if (Mathf.Abs(results[i].x - results[0].x) >= maxXDist)
                    {
                        Vector2 maxVector = results[i];
                        for (; i < steps; i++)
                        {
                            results[i] = maxVector;
                        }
                        break;
                    }
                }

                return results;
            }
        #endregion
    }
}
