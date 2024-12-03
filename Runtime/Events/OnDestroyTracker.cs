using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class OnDestroyTracker : EventTracker
    {
        public Action OnInvoke;

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
