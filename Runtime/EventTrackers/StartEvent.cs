using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class StartEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _startEvent;

        private void Start()
        {
            _startEvent?.Invoke();
        }
    }
}
