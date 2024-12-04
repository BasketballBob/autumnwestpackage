using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class CameraPos : MonoBehaviour
    {
        [SerializeField]
        private float _orthographicSize = 5;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            
        }
    }
}
