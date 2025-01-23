using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class CullingBounds : MonoBehaviour
    {
        private List<CullingObject> _cullingObjects = new List<CullingObject>();
        private Coroutine _cullRoutine;

        protected virtual float CullingRate => .2f;

        protected virtual void Start()
        {
            InitializeVariables();
            _cullRoutine = this.RepeatCoroutineIndefinitely(Cull, AWDelta.DeltaType.FixedUpdate, CullingRate);
        }

        public void AddObject(CullingObject cullingObject)
        {
            if (_cullingObjects.Contains(cullingObject)) return;
            _cullingObjects.Add(cullingObject);
        }

        public void RemoveObject(CullingObject cullingObject)
        {
            if (!_cullingObjects.Contains(cullingObject)) return;
            _cullingObjects.Remove(cullingObject);
        }

        protected IEnumerator Cull()
        {
            for (int i = 0; i < _cullingObjects.Count; i++)
            {
                if (ShouldCull(_cullingObjects[i]))
                {
                    _cullingObjects[i].Cull();
                    i--;
                }
            }

            yield return null;
        }

        protected abstract void InitializeVariables();
        protected abstract bool ShouldCull(CullingObject cullingObject);
    }
}
