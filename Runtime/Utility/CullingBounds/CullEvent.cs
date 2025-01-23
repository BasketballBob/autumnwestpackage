using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class CullEvent : CullingObject
    {
        [SerializeField]
        private UnityEvent _event;

        public override void Cull()
        {
            _event?.Invoke();
        }
    }
}
