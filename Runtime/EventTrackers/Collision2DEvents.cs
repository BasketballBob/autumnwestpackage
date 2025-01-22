using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class Collision2DEvents : MonoBehaviour
    {
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionEnter = new UnityEvent<Collision2D>();
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionStay = new UnityEvent<Collision2D>();
        [FoldoutGroup("Events")]
        public UnityEvent<Collision2D> OnCollisionExit = new UnityEvent<Collision2D>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollisionEnter?.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            OnCollisionStay?.Invoke(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            OnCollisionExit?.Invoke(other);
        }
    }
}
