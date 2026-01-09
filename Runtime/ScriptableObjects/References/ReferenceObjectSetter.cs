using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class ReferenceObjectSetter<TReference> : MonoBehaviour where TReference: class
    {
        [SerializeField]
        private ReferenceObject<TReference> _referenceObject;

        private void Awake()
        {
            if (_referenceObject.Reference == null)
            {
                TReference component = GetComponent<TReference>();
                if (component != null) _referenceObject.Reference = component;
            }
        }

        private void OnDestroy()
        {
            if (_referenceObject.Reference == this)
            {
                _referenceObject.Reference = null;
            }
        }
    }
}
