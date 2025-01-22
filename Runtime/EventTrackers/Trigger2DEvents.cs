using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace AWP
{
    public class Trigger2DEvents : MonoBehaviour
    {
        [FoldoutGroup("Events")]
        public UnityEvent<Collider2D> OnTriggerEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<Collider2D> OnTriggerStay;
        [FoldoutGroup("Events")]
        public UnityEvent<Collider2D> OnTriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("E");
            OnTriggerEnter?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("E2");
            OnTriggerStay?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("E3");
            OnTriggerExit?.Invoke(other);
        }
    }
}
