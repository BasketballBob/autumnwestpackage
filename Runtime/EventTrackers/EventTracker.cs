using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class EventTracker : MonoBehaviour
    {
        protected bool _active = true;

        protected void ActivateAction(UnityEvent action)
        {
            if (!_active) return;
            action?.Invoke();
        }
    }
}
