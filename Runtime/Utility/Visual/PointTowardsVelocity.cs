using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PointTowardsVelocity : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private float _angleOffset;

        private void Update()
        {
            SyncAngle();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Vector2.zero, new Vector2(Mathf.Cos(_angleOffset * Mathf.Deg2Rad), Mathf.Sin(_angleOffset * Mathf.Deg2Rad)));
        }

        private void SyncAngle()
        {
            transform.eulerAngles = new Vector3(0, 0, _rb.velocity.GetEulerZ() + _angleOffset);
        }
    }
}
