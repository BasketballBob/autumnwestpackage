using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PointAtCamera : MonoBehaviour
    {
        public bool LimitAngle = false;
        public float LimitMaxDegrees = 30;

        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.rotation;
        }

        private void Update()
        {
            Point();
        }

        private void Point()
        {
            //Vector2 pointDir = AWGameManager.Camera.transform.position - transform.position;
            if (AWGameManager.AWCamera.Camera == null) return;


            transform.LookAt(AWGameManager.AWCamera.Camera.transform.position);
            if (LimitAngle) 
            {
                transform.rotation = Quaternion.RotateTowards(_initialRotation, transform.rotation, LimitMaxDegrees);
            }
        }
    }
}
