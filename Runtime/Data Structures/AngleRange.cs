using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public struct AngleRange
    {
        public float MinAngle;
        public float MaxAngle;

        public AngleRange(float minAngle, float maxAngle)
        {
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }

        /// <summary>
        /// Checks if provided angle is within the AngleRange
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public bool InRange(float angle)
        {
            if (angle < MinAngle) return false;
            if (angle > MaxAngle) return false;

            return true;
        }

        /// <summary>
        /// Clamps the provided angle by the range
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public float ClampAngle(float angle)
        {
            return Mathf.Clamp(angle, MinAngle, MaxAngle);
        }
    }
}
