using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PositionLooper : MonoBehaviour
    {
        [SerializeField]
        private Transform _trans;
        [SerializeField]
        private Vector3 _loopSize = Vector3.one;
        [SerializeField]
        private float _speedMultiplier = 1;

        private Vector3 _initialPosition;

        public Vector3 Speed { get; set; }

        private void Start()
        {
            _initialPosition = _trans.localPosition;
        }

        private void Update()
        {
            _trans.localPosition += Speed * _speedMultiplier * Time.deltaTime;
            ClampPosition();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, _loopSize);
        }

        private void ClampPosition()
        {
            Vector3 zeroPosition = _trans.localPosition - _initialPosition;
            while (zeroPosition.x < 0) zeroPosition += new Vector3(_loopSize.x, 0, 0);
            while (zeroPosition.x > _loopSize.x) zeroPosition -= new Vector3(_loopSize.x, 0, 0);
            while (zeroPosition.y < 0) zeroPosition += new Vector3(0, _loopSize.y, 0);
            while (zeroPosition.y > _loopSize.y) zeroPosition -= new Vector3(0, _loopSize.y, 0);
            while (zeroPosition.z < 0) zeroPosition += new Vector3(0, 0, _loopSize.z);
            while (zeroPosition.z > _loopSize.z) zeroPosition -= new Vector3(0, 0, _loopSize.z);
            
            _trans.localPosition = zeroPosition + _initialPosition;
        }
    }
}
