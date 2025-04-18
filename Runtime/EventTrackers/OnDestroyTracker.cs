using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class OnDestroyTracker : EventTracker
    {
        public UnityEvent OnInvoke = new UnityEvent();

        private void OnDestroy()
        {
            ActivateAction(OnInvoke);
        }

        public void DestroyWithoutActivating()
        {
            _active = false;
            Destroy(this);
        }
    }
}
