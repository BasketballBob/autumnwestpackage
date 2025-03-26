using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class UnityEventCaller : MonoBehaviour
    {
        [SerializeField]
        private float _eventDelay = 0;
        [SerializeField]
        private bool _destroyOnInvoke;
        [SerializeField]
        private UnityEvent _unityEvent;

        private SingleCoroutine _eventRoutine;

        private void Start()
        {
            _eventRoutine = new SingleCoroutine(this);
        }

        public void Invoke()
        {
            _eventRoutine.StartRoutine(EventRoutine());
        }

        private IEnumerator EventRoutine()
        {
            yield return new WaitForSeconds(_eventDelay);
            _unityEvent.Invoke();
            if (_destroyOnInvoke) Destroy(this);
        }
    }
}
