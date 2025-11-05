using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class RendererDebug : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private Renderer _renderer;

        [ShowInInspector]
        private Bounds _bounds;

        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnDrawGizmosSelected()
        {
            _bounds = _renderer.bounds;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
    }
}
