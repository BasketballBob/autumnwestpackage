using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PointAtCamera : MonoBehaviour
    {
        private void Update()
        {
            Point();
        }

        private void Point()
        {
            //Vector2 pointDir = AWGameManager.Camera.transform.position - transform.position;
            transform.LookAt(AWGameManager.Camera.transform.position);
        }
    }
}
