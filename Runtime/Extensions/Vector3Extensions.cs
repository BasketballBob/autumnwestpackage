using System;
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

        public static Vector3 SetX(this Vector3 vector3, float xPos)
        {
            return new Vector3(xPos, vector3.y, vector3.z);
        }

        public static Vector3 SetY(this Vector3 vector3, float yPos)
        {
            return new Vector3(vector3.x, yPos, vector3.z);
        }

        public static Vector3 SetZ(this Vector3 vector3, float zPos)
        {
            return new Vector3(vector3.x, vector3.y, zPos);
        }

        /// <summary>
        /// Modifies Vector using the provided function and combined various axis of the vector
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Vector3 AxisFunc(this Vector3 vector1, Vector3 vector2, Func<float, float, float> func)
        {
            return new Vector3(func(vector1.x, vector2.x), func(vector1.y, vector2.y), func(vector1.z, vector2.z));
        }

        /// <summary>
        /// Lerps vector1 to vector2 using individual values for each axis
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="deltaVector"></param>
        /// <returns></returns>
        public static Vector3 AxisLerp(this Vector3 vector1, Vector3 vector2, Vector3 deltaVector)
        {
            return new Vector3(Mathf.Lerp(vector1.x, vector2.x, deltaVector.x),
                Mathf.Lerp(vector1.y, vector2.y, deltaVector.y),
                Mathf.Lerp(vector1.z, vector2.z, deltaVector.z));
        }

        /// <summary>
        /// Multiply each axis of vector1 by each corresponding axis of vector2
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector3 AxisMultiply(this Vector3 vector1, Vector3 vector2)
        {
            return AxisFunc(vector1, vector2, (x, y) => x * y);
        }
    }
}
