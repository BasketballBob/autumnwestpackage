using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class Vector2Extensions
    {
        public static float GetEulerZ(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector2 Inverse(this Vector2 vector)
        {
            return new Vector2(-vector.x, -vector.y);
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector2 RotateZAxis(this Vector2 vector, float degrees)
        {
            return Quaternion.AngleAxis(degrees, Vector3.forward) * vector;
        }

        public static Vector2 Lerp(this Vector2 vector1, Vector2 vector2, float delta)
        {
            return vector1 + (vector2 - vector1) * Mathf.Clamp01(delta);
        }

        /// <summary>
        /// Lerps vector1 to vector2 using individual delta values for each axis
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Vector2 AxisLerp(this Vector2 vector1, Vector2 vector2, Vector2 delta)
        {
            return new Vector2(Mathf.Lerp(vector1.x, vector2.x, delta.x),
                Mathf.Lerp(vector1.y, vector2.y, delta.y));
        }

        /// <summary>
        /// Rounds each axis of the Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector2 Round(this Vector2 vector, float digits)
        {
            return vector.AxisFunc((x) => AWMath.Round(x));
        }

        /// <summary>
        /// Gets the absolute value of this vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 Absolute(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        /// <summary>
        /// Modifies each axis with the func
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Vector2 AxisFunc(this Vector2 vector, Func<float, float> func)
        {
            return new Vector2(func(vector.x), func(vector.y));
        }

        /// <summary>
        /// Modifies Vector using the provided function and combined various axis of the vector
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Vector2 AxisFunc(this Vector2 vector1, Vector2 vector2, Func<float, float, float> func)
        {
            return new Vector2(func(vector1.x, vector2.x), func(vector1.y, vector2.y));
        }

        /// <summary>
        /// Gets the vector2 perpendicular to this one in a clock-wise direction
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 PerpendicularClockwise(this Vector2 vector)
        {
            return new Vector2(vector.y, -vector.x);
        }

        /// <summary>
        /// Gets the vector2 perpendicular to this one in a counter clock-wise direction
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        }

        public static Vector3[] ToVector3Array(this Vector2[] array)
        {
            Vector3[] returnArray = new Vector3[array.Length];
            for (int i = 0; i < returnArray.Length; i++) returnArray[i] = array[i];
            return returnArray;
        }
    }
}
