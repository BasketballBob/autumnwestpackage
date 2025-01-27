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

        public static void ArcTowards(this Rigidbody2D rb, Vector2 target, float magnitude)
        {
            // Vector2 offset = target - rb.position;
            // Vector2 

            // if (offset.x == 0) return;
            // float duration = xSpeed / offset.x;


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

            rb.centerOfMass = centerOfMass;
            rb.mass = totalMass;
        }
    }
}
