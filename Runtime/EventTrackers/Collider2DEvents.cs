using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace AWP
{
    public class Collider2DEvents : MonoBehaviour
    {
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionStay;
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnter?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStay?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExit?.Invoke(collision);
        }
    }
}
