using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class CullingObject : MonoBehaviour
    {
        public Action OnCull;

        private bool _invokeCullEvent = true;

        protected virtual void OnEnable()
        {
            AWGameManager.CullingBounds.AddObject(this);
        }

        protected virtual void OnDisable()
        {
            AWGameManager.CullingBounds.RemoveObject(this);
        }

        public virtual void Cull()
        {
            if (_invokeCullEvent) OnCull?.Invoke();
        }

        /// <summary>
        /// Cull without calling the OnCull event
        /// Used to prevent infinite loops when using cull calls to trigger other cull calls (EX: SyncedCullingGroup)
        /// </summary>
        public void CullSilently()
        {
            _invokeCullEvent = false        ;
            Cull();
            _invokeCullEvent = true;
        }
    }
}
