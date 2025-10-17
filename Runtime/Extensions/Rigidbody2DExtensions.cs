using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class Rigidbody2DExtensions
    {
        /// <summary>
        /// Just sends the Rigidbody2D in the direction of the target at magnitude
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="target"></param>
        /// <param name="magnitude"></param>
        public static void LaunchTowards(this Rigidbody2D rb, Vector2 target, float magnitude)
        {   
            rb.velocity = (target - rb.position).normalized * magnitude;
        }

        // public static void ArcTowards(this Rigidbody2D rb, Vector2 target, float magnitude)
        // {
        //     // Vector2 offset = target - rb.position;
        //     // Vector2 

        //     // if (offset.x == 0) return;
        //     // float duration = xSpeed / offset.x;


        // }

        public static Collider2D[] GetAttachedColliders(this Rigidbody2D rb)
        {
            Collider2D[] colliders = new Collider2D[rb.attachedColliderCount];
            rb.GetAttachedColliders(colliders);
            return colliders;
        }

        /// <summary>
        /// Rotates the Rigidbody towards the destination rotation
        /// Just like the balancing body (Crawling Angels)
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="targetRotation"></param>
        /// <param name="maxDegreesDelta"></param>
        public static void BalanceTowards(this Rigidbody2D rb, Quaternion targetRotation, float maxDegreesDelta)
        {
            rb.MoveRotation(Quaternion.RotateTowards(rb.transform.rotation,
                targetRotation, maxDegreesDelta * Time.fixedDeltaTime));
        }

        public static void BalanceCenterOfMass(this Rigidbody2D rb, List<Tuple<Transform, float>> massPoints)
        {
            Vector2 centerOfMass = Vector2.zero;
            float totalMass = 0;

            massPoints.ForEach(x =>
            {
                centerOfMass += (Vector2)(x.Item1.transform.position - rb.transform.position) * x.Item2;
                totalMass += x.Item2;
            });

            rb.centerOfMass = centerOfMass / totalMass;
            rb.mass = totalMass;
        }

        /// <summary>
        /// THIS IS A PSEUDO CALCULATION FOR CENTRIPETAL FORCE
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="point"></param>
        /// <returns></returns>lForceDirection(this Rigidbody2D rb, Vector2 point)
        public static Vector2 GetPseudoCentripetalForce(this Rigidbody2D rb, Vector2 point)
        {
            Vector2 pointVelocity = rb.GetPointVelocity(point);
            Vector2 centripetalForce = pointVelocity.PerpendicularClockwise();
            if (rb.angularVelocity > 0) centripetalForce = pointVelocity.PerpendicularCounterClockwise();
            
            Debug.DrawRay(point, pointVelocity, Color.red);
            Debug.DrawRay(point, -centripetalForce, Color.green);

            return centripetalForce;
        }

        /// <summary>
        /// THIS IS A PSEUDO CALCULATION FOR CENTRIFUGAL FORCE
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="point"></param>
        /// <returns></returns>lForceDirection(this Rigidbody2D rb, Vector2 point)
        public static Vector2 GetPseudoCentrifugalForce(this Rigidbody2D rb, Vector2 point)
        {
            return -rb.GetPseudoCentripetalForce(point);
        }
    }
}
