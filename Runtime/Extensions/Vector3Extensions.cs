using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class Vector3Extensions
    {
        public static float GetEulerZ(this Vector3 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector3 Inverse(this Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }

        public static Vector3 SetXY(this Vector3 vector3, Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, vector3.z);
        }
    }
}
