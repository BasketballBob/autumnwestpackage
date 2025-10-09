using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Used to keep track of all of the rigidbodies inside of the trigger2d
    /// </summary>
    public class TriggerRigidbody2DTracker : RigidbodyTracker<Rigidbody2D>
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"BODY ENTER {other.name} to {name}");
            if (!ColliderIsValid(other)) return;
            OnRigidbodyEnter(other.attachedRigidbody);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"BODY EXIT {other.name} from {name}");
            if (!ColliderIsValid(other)) return;
            OnRigidbodyExit(other.attachedRigidbody);
        }
    }
}
