using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class EventTracker : MonoBehaviour
    {
        protected bool _active = true;

        protected void ActivateAction(Action action)
        {
            if (!_active) return;
            action?.Invoke();
        }
    }
}
