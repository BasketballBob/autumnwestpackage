using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class CullingBounds : MonoBehaviour
    {
        private Coroutine _cullRoutine;

        protected virtual float CullingRate => .1f;

        private void Start()
        {
            _cullRoutine = this.RepeatCoroutineIndefinitely(Cull, AWDelta.DeltaType.FixedUpdate, CullingRate);
        }

        private void Cull()
        {
            Debug.Log("CULL");
        }
    }
}
