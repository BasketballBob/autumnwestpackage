using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Used to keep track of all of the colliders that are inside of the trigger2d
    /// </summary>
    public class Trigger2DColliderTracker : ColliderTracker<Collider2D>
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            OnColliderEnter(other);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            OnColliderExit(other);
        }
    }
}
