using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class UnityEventCaller : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _unityEvent;

        public void Invoke() => _unityEvent.Invoke();
    }
}
