using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Used to group culling objects together and cull all of them when one is culled
    /// (Useful for attached group of rigidbodies, EX: The fish in ChickenChoker)
    /// </summary>
    public class SyncedCullingGroup : MonoBehaviour
    {
        [SerializeField]
        private List<CullingObject> _cullingObjects = new List<CullingObject>();

        private void OnEnable()
        {
            _cullingObjects.ForEach(x => x.OnCull += CullAll);
        }

        private void OnDisable()
        {
            _cullingObjects.ForEach(x => x.OnCull -= CullAll);
        }

        private void CullAll()
        {
            _cullingObjects.ForEach(x => x.CullSilently());
        }
    }
}
