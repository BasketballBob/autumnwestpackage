using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TriggerTracker2D : TriggerTracker<Collider2D>
    {
        [SerializeField]
        protected LayerMask _layerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((_layerMask.value & (1 << other.gameObject.layer)) == 0) return;
            RegisterOther(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((_layerMask.value & (1 << other.gameObject.layer)) == 0) return;
            UnregisterOther(other);
        }
    }
}
