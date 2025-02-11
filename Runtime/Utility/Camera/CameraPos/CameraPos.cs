using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    public class CameraPos : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _referenceResolution = new Vector2(1600, 900);
        [SerializeField]
        private float _orthographicSize = 5;
        [SerializeField]
        private Color _gizmoColor = Color.yellow;

        public float OrthographicSize => _orthographicSize;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector2(_orthographicSize * 
                (_referenceResolution.x / _referenceResolution.y), _orthographicSize) * 2);
        }
    }
}
