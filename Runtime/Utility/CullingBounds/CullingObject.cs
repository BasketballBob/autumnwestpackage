using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class CullingObject : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            AWGameManager.CullingBounds.AddObject(this);
        }

        protected virtual void OnDisable()
        {
            AWGameManager.CullingBounds.RemoveObject(this);
        }

        public abstract void Cull();
    }
}
