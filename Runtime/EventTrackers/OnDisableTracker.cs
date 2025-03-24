using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class OnDisableTracker : EventTracker
    {
        public UnityEvent OnInvoke = new UnityEvent();

        private void OnDisable()
        {
            ActivateAction(OnInvoke);
        }
    }
}
